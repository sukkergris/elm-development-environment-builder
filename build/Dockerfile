FROM debian:12-slim

# Set up the working directory
WORKDIR /tmp

# Set environment variables
ENV USERNAME=container-user \
    HOME_DIR="/home/container-user" \
    PATH="$PATH:/home/container-user/.local/bin:/opt/venv/bin:/home/container-user/.dotnet/tools" \
    DEBIAN_FRONTEND=noninteractive \
    ENV="/home/container-user/.bashrc"

# Install dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    python3 \
    python3-pip \
    python3-venv \
    gcc \
    libc6-dev \
    python3-dev \
    libffi-dev \
    libssl-dev \
    cargo \
    make \
    yarn \
    git \
    openssh-client \
    neovim \
    xclip \
    ripgrep \
    build-essential \
    bash \
    zsh \
    tree \
    stow \
    unzip \
    tmux \
    grep \
    jq \
    curl \
    wget \
    nodejs \
    npm \
    sudo \
    ca-certificates \
    gnupg \
    lsb-release && \
    rm -rf /var/lib/apt/lists/*

# Install .NET 9 SDK via script
# Install .NET 9 SDK to a shared location
ENV DOTNET_ROOT=/opt/dotnet
ENV PATH=$PATH:/opt/dotnet:/opt/dotnet/tools

RUN curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh \
    && chmod +x dotnet-install.sh \
    && ./dotnet-install.sh --version 9.0.306 --install-dir $DOTNET_ROOT \
    && rm dotnet-install.sh

ENV PATH="$PATH:/opt/dotnet/tools"

# Create non-root user and home directory
ARG USER_UID=1000
ARG USER_GID=1000
RUN addgroup --system $USERNAME && \
    adduser --system --ingroup $USERNAME --shell /bin/sh --home $HOME_DIR $USERNAME && \
    mkdir -p $HOME_DIR && \
    chown -R $USERNAME:$USERNAME $HOME_DIR

# Install Elm and supporting tools
ARG ELM_VERSION=latest-0.19.1
ARG ELM_TEST_VERSION=latest-0.19.1
ARG ELM_FORMAT_VERSION=latest-0.19.1
RUN curl -L -o elm.gz https://github.com/elm/compiler/releases/download/0.19.1/binary-for-linux-64-bit.gz && \
    gunzip elm.gz && \
    chmod +x elm && \
    mv elm /usr/local/bin/elm && \
    npm install --global \
        elm-test@$ELM_TEST_VERSION \
        elm-format@$ELM_FORMAT_VERSION \
        elm-watch@beta


# Switch to non-root user
USER $USERNAME

# Install Oh My Zsh and Powerlevel10k theme
RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" || true && \
    mkdir -p $HOME_DIR/.oh-my-zsh/custom/themes && \
    git clone --depth=1 https://github.com/romkatv/powerlevel10k.git $HOME_DIR/.oh-my-zsh/custom/themes/powerlevel10k

# Install dotnet tool
RUN dotnet tool install --global csharp-ls

ENV PATH="$PATH:~/.dotnet/tools"

# Switch back to root for final setup or cleanup
USER root
