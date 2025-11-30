FROM debian:12-slim

# Use bash for the shell (default is sh)
SHELL ["/bin/bash", "-o", "pipefail", "-c"]
# Set environment variables
ENV USERNAME=container-user \
    HOME_DIR="/home/container-user" \
    DOTNET_ROOT=/opt/dotnet \
    DEBIAN_FRONTEND=noninteractive \
    NODE_VERSION=24.11.1

# Update PATH once with all directories
ENV PATH="/usr/local/bin:/opt/dotnet:/opt/dotnet/tools:/home/container-user/.local/bin:/home/container-user/.dotnet/tools:$PATH"

# Install all system dependencies FIRST
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    # DON'T EVER INSTALL sudo FOR A PRODUCTION CONTAINER!
    sudo \
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
    ca-certificates \
    gnupg \
    lsb-release && \
    rm -rf /var/lib/apt/lists/*

# Create non-root user AFTER sudo is installed
ARG USER_UID=1000
ARG USER_GID=1000
RUN groupadd --gid $USER_GID $USERNAME && \
    useradd --uid $USER_UID --gid $USER_GID --create-home --shell /bin/bash $USERNAME && \
    # Remove pw from every sudo making me able to play more. Requires sudo is installed. DON'T EVER USE FOR PRODUCTION!
    echo "$USERNAME ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers

# Install latest nvim
RUN NVIM_FILE="nvim-linux-arm64.tar.gz" && \
    echo "Installing Neovim for ARM64 (Mac M3)..." && \
    curl -LO "https://github.com/neovim/neovim/releases/latest/download/${NVIM_FILE}" && \
    tar -C /opt -xzf "${NVIM_FILE}" && \
    rm "${NVIM_FILE}" && \
        # Link the binary to /usr/local/bin so it's globally available - remember to add '/usr/local/bin/' to path!
    ln -s /opt/nvim-linux-arm64/bin/nvim /usr/local/bin/nvim

# Install .NET SDK (consolidated into one layer, fixed version)
RUN curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh && \
    chmod +x dotnet-install.sh && \
    ./dotnet-install.sh --version 10.0.100 --install-dir $DOTNET_ROOT && \
    ./dotnet-install.sh --runtime aspnetcore --channel 9.0 --install-dir $DOTNET_ROOT && \
    # ./dotnet-install.sh --version 9.0.307 --install-dir $DOTNET_ROOT && \
    rm dotnet-install.sh

# Install Elm and tools (consolidated, removed unnecessary args)
RUN curl -L -o elm.gz https://github.com/elm/compiler/releases/download/0.19.1/binary-for-linux-64-bit.gz && \
    gunzip elm.gz && \
    chmod +x elm && \
    mv elm /usr/local/bin/elm && \
    curl -sL https://taskfile.dev/install.sh | sh -s -- -d -b /usr/local/bin

# Switch to non-root user for remaining setup
USER $USERNAME
WORKDIR $HOME_DIR

# Install nvm + Node and expose binaries system-wide
ENV NVM_DIR=/home/${USERNAME}/.nvm
RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.3/install.sh | bash && \
    . "$NVM_DIR/nvm.sh" && \
    nvm install "$NODE_VERSION" && \
    nvm alias default "$NODE_VERSION" && \
    nvm use default && \
    node_bin="$NVM_DIR/versions/node/$(nvm version default)/bin" && \
    sudo ln -sf "$node_bin/node" /usr/local/bin/node && \
    sudo ln -sf "$node_bin/npm"  /usr/local/bin/npm && \
    sudo ln -sf "$node_bin/npx"  /usr/local/bin/npx

# Install user-specific tools in one layer
RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" && \
    git clone --depth=1 https://github.com/romkatv/powerlevel10k.git $HOME_DIR/.oh-my-zsh/custom/themes/powerlevel10k && \
    dotnet tool install --global csharp-ls && \
    dotnet tool install --global dotnet-script && \
    dotnet tool install --global csharpier && \
    dotnet tool install --global dotnet-format && \
    npm install --global \
    elm-test@latest-0.19.1 \
    elm-format@latest-0.19.1 \
    elm-watch@beta

# Set final working directory
WORKDIR /workspace
