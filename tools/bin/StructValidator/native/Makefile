MKFILE_PATH := $(abspath $(lastword $(MAKEFILE_LIST)))
MKFILE_DIR := $(dir $(MKFILE_PATH))

BINDIR := $(MKFILE_DIR)
TARGET ?= $(MKFILE_DIR)/structcheck

CC := gcc

SRCS = $(wildcard $(MKFILE_DIR)/*.c)
INCS = $(wildcard $(MKFILE_DIR)/*.h)
OBJS = $(patsubst %.c,%.o,$(SRCS))

include auto-generated.mk

PKGCONFIG = $(if $(strip $(DEPS)), `pkg-config --cflags $(DEPS)`,)

CFLAGS += -fPIC -I$(MKFILE_DIR) $(PKGCONFIG)
LDFLAGS +=

$(TARGET): $(OBJS)
	@mkdir -p $(BINDIR)
	@$(CC) -o $@ $^ $(LDFLAGS)

%.o: %.c $(INCS)
	@$(CC) -c -o $@ $< $(CFLAGS)

.PHONY: clean
clean:
	@rm -f $(TARGET)
	@find $(MKFILE_DIR) -type f -name '*.o' -delete
