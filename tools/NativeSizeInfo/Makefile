MKFILE_PATH := $(abspath $(lastword $(MAKEFILE_LIST)))
MKFILE_DIR := $(dir $(MKFILE_PATH))

TARGET := gensizedata
BINDIR := $(MKFILE_DIR)
DATAFILE ?= $(MKFILE_DIR)/native-size.dat

CC := gcc

SRCS = $(wildcard $(MKFILE_DIR)/*.c) $(wildcard $(MKFILE_DIR)/providers/*.c)
INCS = $(wildcard $(MKFILE_DIR)/*.h)
OBJS = $(patsubst %.c,%.o,$(SRCS))

DEPS = elementary

CFLAGS += -fPIC -I$(MKFILE_DIR) `pkg-config --cflags $(DEPS)`
LDFLAGS +=

.PHONY: all
all: $(DATAFILE)

$(DATAFILE): $(BINDIR)/$(TARGET)
	@$(BINDIR)/$(TARGET) > $(DATAFILE)

$(BINDIR)/$(TARGET): $(OBJS)
	@mkdir -p $(BINDIR)
	$(CC) -o $@ $^ $(LDFLAGS)

%.o: %.c $(INCS)
	$(CC) -c -o $@ $< $(CFLAGS)

.PHONY: clean
clean:
	@rm -f $(BINDIR)/$(TARGET)
	@rm -f $(DATAFILE)
	@find $(MKFILE_DIR) -type f -name '*.o' -delete
