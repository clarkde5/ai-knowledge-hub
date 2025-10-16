# 🌤️ Weather MCP Server (Python)

This project is a containerized Python implementation of an [MCP server](https://modelcontextprotocol.info/) using the `weather` example from the [MCP Quickstart Resources](https://github.com/modelcontextprotocol/quickstart-resources/tree/main/weather-server-python). It supports development via **VS Code Dev Containers** and runs the server using the **STDIO transport**, suitable for integration with tools like **Claude**.

---

## 📁 Project Structure

```
.
├── .devcontainer/
│   ├── devcontainer.json        # VS Code Dev Container config
│   ├── Dockerfile               # Multi-stage Dockerfile (dev/runtime)
│   ├── provision.sh             # Additional container setup script
├── install.sh                   # Offline NVM installer (v0.40.3)
├── weather/
│   ├── pyproject.toml           # Python project configuration
│   ├── uv.lock                  # uv dependency lock file
│   ├── weather.py               # MCP weather server implementation
│   └── README.md                # (Optional) Local README for the weather project
```

---

## 🧑‍💻 Development in VS Code (Dev Containers)

This project is fully set up to use [VS Code Dev Containers](https://code.visualstudio.com/docs/devcontainers/containers).

### ✅ Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Dev Containers extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers)

### 🚀 Steps to Get Started

1. **Open the project in VS Code**  
   VS Code should prompt to reopen in a Dev Container. If not, open the Command Palette and choose:

   ```
   Dev Containers: Reopen in Container
   ```

2. **Create and sync the virtual environment**

   Inside the Dev Container terminal:

   ```bash
   cd weather
   uv venv       # Create the virtual environment
   uv sync       # Install dependencies from pyproject.toml and uv.lock
   ```

3. **Run the MCP server in dev mode**

   ```bash
   uv run mcp dev weather.py
   ```

  This starts the [MCP Inspector](https://modelcontextprotocol.io/docs/tools/inspector) with the weather server using **STDIO** transport.

---

## 🐳 Alternative: Run Dev Environment Using Docker Directly

If you prefer not to use VS Code Dev Containers, you can build and run the development environment manually using Docker:

### 🔧 Build the dev image

```bash
docker build --target dev -f .devcontainer/Dockerfile -t weather-mcp-dev .
```

### 🏃 Run the container

Make sure to mount the repo root into the container and set the working directory to the example project path:

```bash
docker run -it --rm \
  -v ../../../:/workspaces/ai-knowledge-hub \
  -w /workspaces/ai-knowledge-hub/examples/tools/python-mcp \
  weather-mcp-dev /bin/bash
```

Once inside the container, set up the Python environment:

```bash
cd weather
uv venv
uv sync
uv run mcp dev weather.py
```

This replicates the VS Code Dev Container environment using only Docker and a terminal.

---

## 🐳 Building and Running the Runtime Container

The Dockerfile supports multi-stage builds. To build and run the production (runtime) container:

### 🔧 Build the runtime image

```bash
docker build --target runtime -f .devcontainer/Dockerfile -t weather-mcp-runtime .
```

### ▶️ Run the container

```bash
docker run -i --rm weather-mcp-runtime
```

This will launch the MCP server over `STDIO` — ready to receive requests from a client like Claude.

---


## 🤖 Claude Integration

To use this server from **Claude Desktop**, add the following to your `claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "Weather MCP Server": {
      "command": "docker",
      "args": [
        "run",
        "-i",
        "--rm",
        "weather-mcp-runtime"
      ]
    }
  }
}
```


You can then prompt Claude with:

> **"Using the Weather MCP Server, what is the weather in Lansing, MI?"**

Claude will forward the request to the containerized MCP server and respond appropriately.

---

## 🤖 GitHub Copilot Integration

You can also use this server as a [custom MCP server in GitHub Copilot](https://code.visualstudio.com/docs/copilot/customization/mcp-servers). Add the following entry inside the `servers` object in your `.vscode/mcp.json`:

```jsonc
{
  "servers": {
    "weather-mcp": {
      "type": "stdio",
      "command": "docker",
      "args": [
        "run",
        "-i",
        "--rm",
        "weather-mcp-runtime"
      ]
    }
  }
}
```


After configuring, you can prompt Copilot with:

> **"Using the weather-mcp server, what is the weather in Lansing, MI?"**

Copilot will forward the request to the containerized MCP server and respond with the result.

For more details, see the [GitHub Copilot MCP Server documentation](https://code.visualstudio.com/docs/copilot/customization/mcp-servers).

---


## 🖥️ Direct STDIO Interaction

When you run the server (e.g., with Docker or Python), it will block and wait for input on stdin. You can interact with the server by typing JSON-RPC messages directly into the terminal. Each message should be a single line of JSON, and you can press Enter to send it. The server will respond on stdout.

**Example session:**

Client (stdin):
```json
{"method":"initialize","params":{"protocolVersion":"2025-06-18","capabilities":{},"clientInfo":{"name":"claude-ai","version":"0.1.0"}},"jsonrpc":"2.0","id":0}
```
Server (stdout):
```json
{"jsonrpc":"2.0","id":0,"result":{"protocolVersion":"2025-06-18","capabilities":{"experimental":{},"prompts":{"listChanged":false},"resources":{"subscribe":false,"listChanged":false},"tools":{"listChanged":false}},"serverInfo":{"name":"weather","version":"1.16.0"}}}
```

Client (stdin):
```json
{"method":"tools/list","params":{},"jsonrpc":"2.0","id":1}
```
Server (stdout):
```json
{"jsonrpc":"2.0","id":1,"result":{"tools":[{"name":"get_alerts","description":"Get weather alerts for a US state. ..."},{"name":"get_forecast","description":"Get weather forecast for a location. ..."}]}}
```


You can continue typing additional JSON-RPC requests as needed. To exit, use Ctrl+C.

---

## 🛠 Additional Notes

- The root-level `install.sh` is an **offline NVM installer** (v0.40.3) used during container provisioning.
- All Python-related code lives in the `./weather` directory.
- This project uses [`uv`](https://github.com/astral-sh/uv) for Python dependency and environment management.
- The server uses **STDIO transport**, as required for integration with Claude and MCP tooling.

---

## 📚 References

- [Model Context Protocol (MCP)](https://modelcontextprotocol.info/)
- [Quickstart: Weather Server Example](https://github.com/modelcontextprotocol/quickstart-resources/tree/main/weather-server-python)
- [uv Python Package Manager](https://github.com/astral-sh/uv)
- [Claude + MCP Integration Docs](https://docs.claude.com/en/docs/mcp)
- [MCP Inspector Documentation](https://modelcontextprotocol.io/docs/tools/inspector)

---

## 📌 TODO (Optional Enhancements)

- Add support for additional MCP transport types
- Add Python client
- Figure out why `install.sh` is required for NVM  
  _(appears to be related to corporate networking restrictions like Zscaler)_

---
