set version=5.0.0
dotnet pack ../src/SystemsRx.MicroRx -c Release -o ../../_dist /p:version=%version%
dotnet pack ../src/SystemsRx -c Release -o ../../_dist /p:version=%version%
dotnet pack ../src/SystemsRx.Infrastructure -c Release -o ../../_dist /p:version=%version%
dotnet pack ../src/SystemsRx.Plugins.Computeds -c Release -o ../../_dist /p:version=%version%
dotnet pack ../src/SystemsRx.Infrastructure -c Release -o ../../_dist /p:version=%version%
dotnet pack ../src/SystemsRx.Infrastructure.Ninject -c Release -o ../../_dist /p:version=%version%
dotnet pack ../src/SystemsRx.ReactiveData -c Release -o ../../_dist /p:version=%version%