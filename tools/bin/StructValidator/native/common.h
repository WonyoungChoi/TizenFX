
#define PV_CONCAT(x, y) _PV_CONCAT(x, y)
#define _PV_CONCAT(x, y) x##y

#define CHECK_STRUCT(N, S, T) _CHECK_STRUCT(N, S, T, PV_CONCAT(__fn_, __COUNTER__))
#define _CHECK_STRUCT(N, S, T, F) __CHECK_STRUCT(N, S, T, F)
#define __CHECK_STRUCT(N, S, T, F)                                 \
    static int F(void);                                            \
    static void __attribute__((constructor)) __construct_##F(void) \
    {                                                              \
        _add_function_to_provider_list(F);                         \
    }                                                              \
    static int F(void)                                             \
    {                                                              \
        return _check_struct(N, S, sizeof(T));                     \
    }

void _add_function_to_provider_list(int (*f)(void));
int _check_struct(const char *name, int managed, int unmanaged);
