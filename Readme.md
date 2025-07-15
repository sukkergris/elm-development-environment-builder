# Elm Development Environment Builder

[![Alpine Linux v3.21](https://img.shields.io/badge/Alpine%20Linux-v3.21-0D597F?style=flat-square&logo=alpine-linux)](https://alpinelinux.org/)
[![Elm 0.19.1](https://img.shields.io/badge/Elm-0.19.1-60B5CC?style=flat-square&logo=elm)](https://elm-lang.org/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=flat-square&logo=docker)](https://www.docker.com/)
[![.NET SDK 9](https://img.shields.io/badge/.NET%20SDK-9-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)

A containerized development environment for Elm projects, running on Alpine Linux. This repository provides everything you need to start building Elm applications with a consistent, reproducible environment.

## 🚀 Features

- **Alpine Linux v3.21** base for lightweight containers
- **Elm 0.19.1** pre-installed with common tools:
  - elm-test
  - elm-format
  - elm-watch (beta)
- **Development Tools**:
  - Neovim with plugins
  - .NET SDK 9
  - Git, SSH, and other essential utilities
  - ZSH with Oh-My-ZSH and Powerlevel10k theme
  - Nushell and Tmux for terminal productivity
  - Azure CLI for cloud operations
- **Containerized workflow** for consistent development across machines
- **VS Code integration** ready (devcontainer compatible)

## 🏗️ Quick Start

### Prerequisites

- [Docker](https://www.docker.com/get-started) installed on your system
- [Docker Compose](https://docs.docker.com/compose/install/) (usually included with Docker Desktop)
- Git for cloning the repository

### Setup and Run

#### Option 1: Use the pre-built Docker image

You can use the pre-built Docker image directly from Docker Hub:

```bash
# Pull the image
docker pull isuperman/elm-devcontainer-foundation:latest

# Run the container
docker run -it --name elm-dev isuperman/elm-devcontainer-foundation:latest zsh
```

Visit the Docker Hub repository for more information: [https://hub.docker.com/repository/docker/isuperman/elm-devcontainer-foundation/general](https://hub.docker.com/repository/docker/isuperman/elm-devcontainer-foundation/general)

#### Option 2: Build from source

1. Clone this repository:

```bash
git clone https://github.com/yourusername/elm-development-environment-builder.git
cd elm-development-environment-builder
```

2. Build and start the container:

```bash
# Set your Docker Hub username and desired image tag
export DOCKER_USERNAME=yourusername
export REPO_NAME=elm-dev-env
export IMAGE_TAG=latest

# Build the image
cd build
docker compose build

# Run the container
docker compose up -d
```

3. Connect to the running container:

```bash
docker exec -it build-app-1 zsh
```

## 📂 Repository Structure

```
├── build/              # Docker container definition
│   ├── docker-compose.yml
│   ├── Dockerfile
│   ├── .env            # Environment variables for Docker Compose
│   └── README.md
├── dotnet/             # .NET project examples and utilities
│   └── test/           # Sample .NET test project
└── elm/                # Elm application source
    ├── elm.json        # Elm project configuration
    └── src/            # Elm source code
        └── Main.elm    # Entry point for Elm application
```

## 🧩 Elm Project Development

To start a new Elm project inside the container:

```bash
cd /workspace/elm
elm init  # If starting fresh
```

Run your Elm application:

```bash
elm reactor
```

To access the Elm application in your browser:

```bash
# From inside the container
$BROWSER http://localhost:8000
```

## 🔧 Customization

### Adding Additional Elm Packages

To add packages to your Elm project:

```bash
cd /workspace/elm
elm install author/package-name
```

### Customizing the Docker Container

Edit the `build/Dockerfile` to add or modify tools and dependencies based on your needs. Then rebuild the image using the docker compose command shown above.

## 💡 Tips for Working with This Environment

- The container user has sudo privileges for installing additional packages
- Use `elm-watch` for hot reloading during development
- The Elm compiler and tools are available globally in the PATH
- Dotnet tools can be accessed through the `dotnet` command

## 🔒 Security Notes

- The container runs with a non-root user `container-user`
- SSH keys are not included and should be mounted as needed
- Container networking is managed through Docker Compose

## 📚 Additional Resources

- [Elm Language Documentation](https://guide.elm-lang.org/)
- [Elm Package Catalog](https://package.elm-lang.org/)
- [Alpine Linux Documentation](https://docs.alpinelinux.org/)
- [Docker Documentation](https://docs.docker.com/)

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## ✨ About This Documentation

This documentation was created entirely by GitHub Copilot, demonstrating the capabilities of AI assistance in software development documentation. All content has been (no so) carefully reviewed by a human to ensure accuracy and completeness.