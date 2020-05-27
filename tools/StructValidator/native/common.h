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
