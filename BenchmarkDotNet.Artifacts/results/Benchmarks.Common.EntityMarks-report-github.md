``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core i5-7200U CPU 2.50GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
| Method |    Job | Runtime |   N | Mean | Error | Ratio | RatioSD | Rank |
|------- |------- |-------- |---- |-----:|------:|------:|--------:|-----:|
| **Update** |    **Clr** |     **Clr** |   **5** |   **NA** |    **NA** |     **?** |       **?** |    **?** |
| Update |   Core |    Core |   5 |   NA |    NA |     ? |       ? |    ? |
| Update | CoreRT |  CoreRT |   5 |   NA |    NA |     ? |       ? |    ? |
|        |        |         |     |      |       |       |         |      |
| **Update** |    **Clr** |     **Clr** |  **50** |   **NA** |    **NA** |     **?** |       **?** |    **?** |
| Update |   Core |    Core |  50 |   NA |    NA |     ? |       ? |    ? |
| Update | CoreRT |  CoreRT |  50 |   NA |    NA |     ? |       ? |    ? |
|        |        |         |     |      |       |       |         |      |
| **Update** |    **Clr** |     **Clr** | **100** |   **NA** |    **NA** |     **?** |       **?** |    **?** |
| Update |   Core |    Core | 100 |   NA |    NA |     ? |       ? |    ? |
| Update | CoreRT |  CoreRT | 100 |   NA |    NA |     ? |       ? |    ? |
|        |        |         |     |      |       |       |         |      |
| **Update** |    **Clr** |     **Clr** | **200** |   **NA** |    **NA** |     **?** |       **?** |    **?** |
| Update |   Core |    Core | 200 |   NA |    NA |     ? |       ? |    ? |
| Update | CoreRT |  CoreRT | 200 |   NA |    NA |     ? |       ? |    ? |

Benchmarks with issues:
  EntityMarks.Update: Clr(Runtime=Clr) [N=5]
  EntityMarks.Update: Core(Runtime=Core) [N=5]
  EntityMarks.Update: CoreRT(Runtime=CoreRT) [N=5]
  EntityMarks.Update: Clr(Runtime=Clr) [N=50]
  EntityMarks.Update: Core(Runtime=Core) [N=50]
  EntityMarks.Update: CoreRT(Runtime=CoreRT) [N=50]
  EntityMarks.Update: Clr(Runtime=Clr) [N=100]
  EntityMarks.Update: Core(Runtime=Core) [N=100]
  EntityMarks.Update: CoreRT(Runtime=CoreRT) [N=100]
  EntityMarks.Update: Clr(Runtime=Clr) [N=200]
  EntityMarks.Update: Core(Runtime=Core) [N=200]
  EntityMarks.Update: CoreRT(Runtime=CoreRT) [N=200]
