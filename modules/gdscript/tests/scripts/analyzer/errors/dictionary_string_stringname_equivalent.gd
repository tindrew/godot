# https://github.com/Redotengine/Redot/issues/62957

func test():
	var dict = {
		&"key": "StringName",
		"key": "String"
	}

	print("Invalid dictionary: %s" % dict)
