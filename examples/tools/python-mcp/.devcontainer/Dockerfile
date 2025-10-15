FROM python:3.11-bookworm AS dev

USER root

COPY .devcontainer/provision.sh /tmp/provision/provision.sh
COPY install.sh /tmp/provision/install.sh
RUN bash /tmp/provision/provision.sh

ENV HOST=0.0.0.0
ENV PATH="/root/.local/bin:${PATH}"

WORKDIR /workspaces

FROM dev AS runtime

WORKDIR /runtime

COPY weather/weather.py /runtime/weather.py
COPY weather/pyproject.toml /runtime/pyproject.toml
COPY weather/uv.lock /runtime/uv.lock

RUN uv venv && uv sync

CMD [".venv/bin/python", "weather.py"]
