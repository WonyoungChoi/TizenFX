#include <time.h>
#include <Elementary.h>

#include "provider.h"

NATIVE_SIZE_OF("Interop/Libc/SystemTime")
{
    return sizeof(struct tm);
}

NATIVE_SIZE_OF("Interop/Eina/Size2D")
{
    return sizeof(Eina_Size2D);
}

NATIVE_SIZE_OF("ElmSharp.EcoreKeyEventArgs/EcoreEventKey")
{
    return sizeof(Ecore_Event_Key);
}

NATIVE_SIZE_OF("ElmSharp.EvasKeyEventArgs/EvasEventKeyDown")
{
}