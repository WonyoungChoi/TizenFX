#include <stdio.h>
#include <stdlib.h>

typedef struct __provider_list_node
{
    struct __provider_list_node *next;
    int (*func)(void);
} _provider_list_node;

_provider_list_node *_provider_list_head = NULL;

void _add_function_to_provider_list(int (*f)(void))
{
    _provider_list_node *node = (_provider_list_node *)malloc(sizeof(_provider_list_node));
    node->func = f;
    node->next = _provider_list_head;
    _provider_list_head = node;
}

int _check_struct(const char *name, int managed, int unmanaged)
{
    if (managed != unmanaged)
    {
        printf("* %s size is mismatch. managed(%d) != unmanaged(%d)\n", name, managed, unmanaged);
        return 0;
    }
    return 1;
}

int main(int argc, char **argv)
{
    int valid = 1;
    _provider_list_node *node = _provider_list_head;
    while (node != NULL)
    {
        valid = valid & node->func();
        node = node->next;
    }
    if (!valid)
    {
        printf("*** Invalid struct size is detected. ***\n");
        return 1;
    }
    return 0;
}
