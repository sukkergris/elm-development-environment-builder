# Use a stable version of Alpine as the base image
FROM alpine:3.21.0

# Set up the working directory
WORKDIR /tmp

# Set environment variables
ENV HOME_DIR="/home/container-user"
# ENV LV_BRANCH="release-1.4/neovim-0.9"
ENV PATH="$PATH:$HOME_DIR/.local/bin"

# Install dependencies
RUN apk update && \
    apk add --no-cache \
    python3 \
    py3-pip \
    gcc \
    musl-dev \
    python3-dev \
    libffi-dev \
    openssl-dev \
    cargo \
    make \
    yarn \
    git \
    openssh \
    neovim \
    neovim-doc \
    xclip \
    ripgrep \
    alpine-sdk \
    dotnet9-sdk \
    bash \
    zsh \
    tree \
    stow \
    unzip \
    nushell \
    tmux \
    grep \
    jq \
    curl \
    nodejs \
    npm \
    sudo \
    libc6-compat  # Install if needed for compatibility with Elm binaries

# Configuring Elm version
ARG ELM_VERSION=latest-0.19.1
ARG ELM_TEST_VERSION=latest-0.19.1
ARG ELM_FORMAT_VERSION=latest-0.19.1

# This Dockerfile adds a non-root user with sudo access.
ARG USERNAME=container-user
ARG USER_UID=1000
ARG USER_GID=$USER_UID

# Add a non-root user and group
RUN addgroup -S $USERNAME && \
    adduser -S $USERNAME -G $USERNAME --shell /bin/sh

# Install Elm using the provided method, elm-test and elm-format via npm
RUN export DEBIAN_FRONTEND=noninteractive && \
    # Install Elm binary
    curl -L -o elm.gz https://github.com/elm/compiler/releases/download/0.19.1/binary-for-linux-64-bit.gz && \
    gunzip elm.gz && \
    chmod +x elm && \
    mv elm /usr/local/bin/elm && \
    # Install elm-test and elm-format via npm
    npm install --global \
        elm-test@${ELM_TEST_VERSION} \
        elm-format@${ELM_FORMAT_VERSION} \
        elm-watch@beta && \
    # [Optional] Update UID/GID if needed
    if [ "$USER_GID" != "1000" ] || [ "$USER_UID" != "1000" ]; then \
        groupmod --gid $USER_GID $USERNAME && \
        usermod --uid $USER_UID --gid $USER_GID $USERNAME && \
        chown -R $USER_UID:$USER_GID /home/$USERNAME; \
    fi && \
    # Create the elm cache directory where we can mount a volume
    mkdir /home/$USERNAME/.elm && \
    chown $USERNAME:$USERNAME /home/$USERNAME/.elm && \
    # Create the .azure directory
    mkdir -p /home/container-user/.azure && \
    chown $USERNAME:$USERNAME /home/$USERNAME/.azure

# https://stackoverflow.com/questions/38024160/how-to-get-etc-profile-to-run-automatically-in-alpine-docker
ENV ENV=/home/$USERNAME/.profile

USER $USERNAME
ARG PULUMI_VERSION=3.143.0
RUN curl -fsSL https://get.pulumi.com/ | bash -s -- --version $PULUMI_VERSION --install-dir /home/container-user/.pulumi
ENV PATH="$PATH:/home/$USERNAME/.pulumi/bin"

RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" || true
RUN git clone --depth=1 https://github.com/romkatv/powerlevel10k.git /home/$USERNAME/.oh-my-zsh/custom/themes/powerlevel10k

RUN dotnet tool install --global csharp-ls
ENV PATH="$PATH:~/.dotnet/tools"
USER root

# Create and activate a virtual environment for Azure CLI
RUN python3 -m venv /opt/venv \
  && . /opt/venv/bin/activate \
  && pip install --upgrade pip \
  && pip install --no-cache-dir azure-cli \
  && deactivate

# Clean up unnecessary build tools
# RUN apk del \
#   gcc \
#   musl-dev \
#   python3-dev \
#   libffi-dev \
#   openssl-dev \
#   cargo \
#   make \
#   && rm -rf /var/cache/apk/*

# Update PATH to include the virtual environment
ENV PATH="/opt/venv/bin:$PATH"
