/**************************************************************************/
/*  register_types.cpp                                                    */
/**************************************************************************/
/*                         This file is part of:                          */
/*                             GODOT ENGINE                               */
/*                        https://godotengine.org                         */
/**************************************************************************/
/* Copyright (c) 2014-present Godot Engine contributors (see AUTHORS.md). */
/* Copyright (c) 2007-2014 Juan Linietsky, Ariel Manzur.                  */
/*                                                                        */
/* Permission is hereby granted, free of charge, to any person obtaining  */
/* a copy of this software and associated documentation files (the        */
/* "Software"), to deal in the Software without restriction, including    */
/* without limitation the rights to use, copy, modify, merge, publish,    */
/* distribute, sublicense, and/or sell copies of the Software, and to     */
/* permit persons to whom the Software is furnished to do so, subject to  */
/* the following conditions:                                              */
/*                                                                        */
/* The above copyright notice and this permission notice shall be         */
/* included in all copies or substantial portions of the Software.        */
/*                                                                        */
/* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,        */
/* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF     */
/* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. */
/* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY   */
/* CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,   */
/* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE      */
/* SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                 */
/**************************************************************************/

#include "register_types.h"

#include "csharp_script.h"

#ifdef TOOLS_ENABLED
#include "editor/editor_translation_parser.h"

#include "editor/csharp_translation_parser_plugin.h"
#endif

CSharpLanguage *script_language_cs = nullptr;
Ref<ResourceFormatLoaderCSharpScript> resource_loader_cs;
Ref<ResourceFormatSaverCSharpScript> resource_saver_cs;

#ifdef TOOLS_ENABLED
Ref<CSharpEditorTranslationParserPlugin> csharp_translation_parser_plugin;
#endif

mono_bind::GodotSharp *_godotsharp = nullptr;

void initialize_mono_module(ModuleInitializationLevel p_level) {
	if (p_level == MODULE_INITIALIZATION_LEVEL_SCENE) {
		GDREGISTER_CLASS(CSharpScript);

		_godotsharp = memnew(mono_bind::GodotSharp);

		script_language_cs = memnew(CSharpLanguage);
		script_language_cs->set_language_index(ScriptServer::get_language_count());
		ScriptServer::register_language(script_language_cs);

		resource_loader_cs.instantiate();
		ResourceLoader::add_resource_format_loader(resource_loader_cs);

		resource_saver_cs.instantiate();
		ResourceSaver::add_resource_format_saver(resource_saver_cs);
	}

#ifdef TOOLS_ENABLED
	if (p_level == MODULE_INITIALIZATION_LEVEL_SERVERS) {
		csharp_translation_parser_plugin.instantiate();
		EditorTranslationParser::get_singleton()->add_parser(csharp_translation_parser_plugin, EditorTranslationParser::STANDARD);
	}
#endif // TOOLS_ENABLED
}

void uninitialize_mono_module(ModuleInitializationLevel p_level) {
	if (p_level != MODULE_INITIALIZATION_LEVEL_SCENE) {
		return;
	}

	ScriptServer::unregister_language(script_language_cs);

	if (script_language_cs) {
		memdelete(script_language_cs);
	}

	ResourceLoader::remove_resource_format_loader(resource_loader_cs);
	resource_loader_cs.unref();

	ResourceSaver::remove_resource_format_saver(resource_saver_cs);
	resource_saver_cs.unref();

#ifdef TOOLS_ENABLED
	if (p_level == MODULE_INITIALIZATION_LEVEL_EDITOR) {
		EditorTranslationParser::get_singleton()->remove_parser(csharp_translation_parser_plugin, EditorTranslationParser::STANDARD);
		csharp_translation_parser_plugin.unref();
	}
#endif // TOOLS_ENABLED

	if (_godotsharp) {
		memdelete(_godotsharp);
	}
}
