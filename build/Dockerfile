FROM debian:12-slim

# Use bash for the shell (default is sh)
SHELL ["/bin/bash", "-o", "pipefail", "-c"]
# Set environment variables
ENV USERNAME=container-user \
    HOME_DIR="/home/container-user" \
    DOTNET_ROOT=/opt/dotnet \
    DEBIAN_FRONTEND=noninteractive

# Update PATH once with all directories
ENV PATH="/opt/dotnet:/opt/dotnet/tools:/home/container-user/.local/bin:/home/container-user/.dotnet/tools:$PATH"

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
    ca-certificates \
    gnupg \
    npm \
    nodejs \
    lsb-release && \
    rm -rf /var/lib/apt/lists/*

# Create non-root user AFTER sudo is installed
ARG USER_UID=1000
ARG USER_GID=1000
RUN groupadd --gid $USER_GID $USERNAME && \
    useradd --uid $USER_UID --gid $USER_GID --create-home --shell /bin/bash $USERNAME && \
    # Remove pw from every sudo making me able to play more. Requires sudo is installed. DON'T EVER USE FOR PRODUCTION!
    echo "$USERNAME ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers



# Install .NET SDK (consolidated into one layer, fixed version)
RUN curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh && \
    chmod +x dotnet-install.sh && \
    ./dotnet-install.sh --version 10.0.100 --channel 9.0 --install-dir $DOTNET_ROOT && \
    rm dotnet-install.sh

# Install Elm and tools (consolidated, removed unnecessary args)
RUN curl -L -o elm.gz https://github.com/elm/compiler/releases/download/0.19.1/binary-for-linux-64-bit.gz && \
    gunzip elm.gz && \
    chmod +x elm && \
    mv elm /usr/local/bin/elm && \
    npm install --global \
        elm-test@latest-0.19.1 \
        elm-format@latest-0.19.1 \
        elm-watch@beta && \
    curl -sL https://taskfile.dev/install.sh | sh -s -- -d -b /usr/local/bin

# Switch to non-root user for remaining setup
USER $USERNAME
WORKDIR $HOME_DIR

# Create a script file sourced by both interactive and non-interactive bash shells
ENV BASH_ENV /home/${USERNAME}/.bash_env
RUN touch "${BASH_ENV}" && \
    echo '. "${BASH_ENV}"' >> ~/.bashrc

# Download and install nvm
RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.3/install.sh | PROFILE="${BASH_ENV}" bash && \
    echo "node" > .nvmrc

# Install user-specific tools in one layer
RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" && \
    git clone --depth=1 https://github.com/romkatv/powerlevel10k.git $HOME_DIR/.oh-my-zsh/custom/themes/powerlevel10k && \
    dotnet tool install --global csharp-ls && \
    dotnet tool install --global dotnet-script && \
    dotnet tool install -g csharpier && \
    dotnet tool install -g dotnet-format

# Set final working directory
WORKDIR /workspace
