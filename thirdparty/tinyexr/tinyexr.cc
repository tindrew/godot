#if defined(_WIN32)
#ifndef NOMINMAX
#define NOMINMAX
#endif
#endif

// -- Redot start --
#include <zlib.h> // Should come before including tinyexr.
// -- Redot end --

#define TINYEXR_IMPLEMENTATION
#include "tinyexr.h"
