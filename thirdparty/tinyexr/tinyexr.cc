#if defined(_WIN32)
#ifndef NOMINMAX
#define NOMINMAX
#endif
#endif

// -- redot start --
#include <zlib.h> // Should come before including tinyexr.
// -- redot end --

#define TINYEXR_IMPLEMENTATION
#include "tinyexr.h"
