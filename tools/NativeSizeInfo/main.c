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
#include <stdlib.h>
#include <string.h>

typedef struct __provider_list_node
{
    struct __provider_list_node *next;
    const char *name;
    int (*func)(void);
} _provider_list_node;

_provider_list_node *_provider_list_head = NULL;

void add_function_to_provider_list(const char *name, int (*f)(void))
{
    _provider_list_node *node = (_provider_list_node *)malloc(sizeof(_provider_list_node));
    node->func = f;
    node->name = name;
    node->next = _provider_list_head;
    _provider_list_head = node;
}

int main(int argc, char **argv)
{
    _provider_list_node *node = _provider_list_head;
    while (node != NULL)
    {
        printf("%s:%d\n", node->name, node->func());
        node = node->next;
    }
}
