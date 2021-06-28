# SystemsRx
A general system execution framework with additional layers for `Application` scenarios with DI abstraction and plugin support.

[![Build Status][build-status-image]][build-status-url]
[![License][license-image]][license-url]
[![Nuget Version][nuget-image]][nuget-url]
[![Join Discord Chat][discord-image]][discord-url]

> This originally was a core part of [EcsRx](https://github.com/EcsRx/ecsrx) however now lives as it's own repo

## Features

- Simple system interfaces and implementations to use/extend
- Fully reactive architecture
- Favours composition over inheritance
- Adheres to inversion of control
- Lightweight codebase
- Built in support for events (raise your own and react to them)
- Built in Dependency Injection abstraction layer
- Built in support for plugins (wrap up your own components/systems/events and share them with others)

## Architecture

The architecture is layered so you can use the core parts without needing the additional layers if you want to keep things bare bones.

### MicroRx

This is a bare bones rx implementation, it literally just contains some basic `Subject` and other related rx classes, this is so that we do not have a dependencies on rx.net or unirx in the core.

### Infrastructure

SystemsRx is a really just the Systems aspect of the ECS paradigm without any dependencies on Entites or Components. It allows you create your own conventions for systems as well as make use of the infrastructure layer which provides a DI abstraction layer, Plugin support and some best practice classes.

The 2 main libraries here are **SystemsRx** and **SystemsRx.Infrastructure**, these only have a dependency on **MicroRx**

[build-status-image]: https://ci.appveyor.com/api/projects/status/6incybkqawq9qe7u?svg=true
[build-status-url]: https://ci.appveyor.com/project/grofit/systemsrx/branch/main
[nuget-image]: https://img.shields.io/nuget/v/systemsrx.svg
[nuget-url]: https://www.nuget.org/packages/SystemsRx/
[discord-image]: https://img.shields.io/discord/488609938399297536.svg
[discord-url]: https://discord.gg/bS2rnGz
[license-image]: https://img.shields.io/github/license/ecsrx/ecsrx.svg
[license-url]: https://github.com/EcsRx/systemsrx/blob/master/LICENSE