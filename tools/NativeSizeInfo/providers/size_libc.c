#include <time.h>

#include "provider.h"

NATIVE_SIZE_OF("Interop/Libc/SystemTime")
{
    return sizeof(struct tm);
}

