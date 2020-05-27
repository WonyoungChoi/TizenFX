/*
 * Copyright (c) 2020 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#include <stdio.h>

void add_function_to_provider_list(const char *name, int (*f)(void));

#define PV_CONCAT(x, y) _PV_CONCAT(x, y)
#define _PV_CONCAT(x, y) x##y

#define NATIVE_SIZE_OF(n) _NATIVE_SIZE_OF(n, PV_CONCAT(__fn_, __COUNTER__))
#define _NATIVE_SIZE_OF(n, f) __NATIVE_SIZE_OF(n, f)
#define __NATIVE_SIZE_OF(n, f)                                     \
    static int f(void);                                            \
    static void __attribute__((constructor)) __construct_##f(void) \
    {                                                              \
        add_function_to_provider_list(n, f);                       \
    }                                                              \
    static int f(void)
