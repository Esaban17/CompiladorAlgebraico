# Compilador Algebraico

Un compilador/interprete de expresiones algebraicas escrito en C# que evalua operaciones matematicas utilizando analisis lexico y sintactico.

[![CI](https://github.com/Esaban17/CompiladorAlgebraico/actions/workflows/ci.yml/badge.svg)](https://github.com/Esaban17/CompiladorAlgebraico/actions/workflows/ci.yml)

## Descripcion

Este proyecto implementa un compilador algebraico que analiza y evalua expresiones matematicas. Utiliza tecnicas de compiladores como analisis lexico (scanner) y analisis sintactico descendente recursivo (parser) para procesar las expresiones.

## Caracteristicas

- Evaluacion de expresiones matematicas
- Operaciones soportadas: suma (`+`), resta (`-`), multiplicacion (`*`) y division (`/`)
- Numeros enteros y decimales (`3.14`, `.5`)
- Manejo de parentesis para agrupar operaciones
- Operador unario negativo, incluso encadenado (`-5`, `--5`, `2 - -3`)
- Precedencia de operadores y asociatividad por la izquierda
- Aritmetica de doble precision (`double`) de extremo a extremo
- Errores lexicos, sintacticos y de evaluacion con la posicion exacta del problema

## Requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (LTS)
- Sistema operativo compatible con .NET (Windows, Linux, macOS)

## Instalacion

1. Clona el repositorio:

```bash
git clone https://github.com/Esaban17/CompiladorAlgebraico.git
cd CompiladorAlgebraico
```

2. Compila el proyecto:

```bash
dotnet build
```

3. Ejecuta el proyecto:

```bash
dotnet run --project CompiladorAlgebraico
```

## Uso

Al ejecutar el programa sin argumentos, se solicita ingresar una expresion matematica. El programa la evalua y muestra el resultado.

Tambien acepta la expresion como argumento de linea de comandos, lo que permite usarlo en scripts:

```bash
dotnet run --project CompiladorAlgebraico -- "2 + 3 * 4"
echo "2 + 3 * 4" | dotnet run --project CompiladorAlgebraico
```

El programa termina con codigo de salida `0` si la expresion se evaluo y `1` si hubo un error. Los errores se escriben en `stderr`.

### Ejemplos de expresiones validas

```
2 + 3 * 4
(5 + 3) * 2
10 - 2 * 3
100 / (2 + 3)
-5 + 10
(2 + 3) * (4 - 1)
1 - 2 - 3
1.5 + 2.5
```

### Ejemplo de ejecucion

```
Ingrese la expresion: 2 + 3 * 4

----------------------------
El resultado es: 14
----------------------------
```

## Estructura del Proyecto

```
CompiladorAlgebraico/
├── CompiladorAlgebraico.sln
├── README.md
├── .github/
│   └── workflows/
│       └── ci.yml
├── CompiladorAlgebraico/
│   ├── CompiladorAlgebraico.csproj
│   ├── Program.cs
│   ├── Scanner.cs
│   ├── Parser.cs
│   ├── Token.cs
│   ├── TokenType.cs
│   └── CompilerException.cs
└── CompiladorAlgebraico.Tests/
    ├── CompiladorAlgebraico.Tests.csproj
    ├── ScannerTests.cs
    └── ParserTests.cs
```

### Componentes

| Archivo | Descripcion |
|---------|-------------|
| **Program.cs** | Punto de entrada. Lee la expresion (de la consola o de los argumentos), invoca al parser y reporta el resultado o el error. |
| **Scanner.cs** | Analizador lexico que convierte la cadena de entrada en tokens. Reconoce numeros enteros y decimales, operadores y parentesis. |
| **Parser.cs** | Analizador sintactico descendente recursivo. Evalua las expresiones respetando la precedencia y la asociatividad de los operadores mediante las funciones `E()`, `T()`, `F()` y `R()`. |
| **Token.cs** | Token inmutable con su tipo (`Tag`), su texto (`Value`) y su posicion en la entrada (`Position`). |
| **TokenType.cs** | Enumeracion de tipos de tokens soportados. |
| **CompilerException.cs** | Jerarquia de errores: `LexicalException`, `SyntacticException` y `EvaluationException`. |

### Tipos de Tokens

| Token | Simbolo | Descripcion |
|-------|---------|-------------|
| `Number` | `0-9`, `.` | Numeros enteros o decimales |
| `Plus` | `+` | Suma |
| `Minus` | `-` | Resta / negacion unaria |
| `Mult` | `*` | Multiplicacion |
| `Div` | `/` | Division |
| `LParen` | `(` | Parentesis izquierdo |
| `RParen` | `)` | Parentesis derecho |
| `EOF` | — | Fin de entrada |

## Gramatica

El parser implementa la siguiente gramatica, que respeta la precedencia de operadores:

```
E -> T (('+' | '-') T)*
T -> F (('*' | '/') F)*
F -> '-' F | R
R -> number | '(' E ')'
```

Donde:

- **E**: Expresion (maneja suma y resta)
- **T**: Termino (maneja multiplicacion y division)
- **F**: Factor (maneja el operador unario negativo, que puede encadenarse)
- **R**: Elemento basico (numeros o expresiones entre parentesis)

La repeticion con `*` reemplaza a los no terminales `E'` y `T'` de la formulacion recursiva pura
(`E' -> + T E' | - T E' | e`). Ambas reconocen el mismo lenguaje, pero acumular en un bucle es lo
que da la **asociatividad por la izquierda**: `1 - 2 - 3` se agrupa como `(1 - 2) - 3 = -4`, no como
`1 - (2 - 3) = 2`.

## Manejo de Errores

El compilador detecta y reporta tres tipos de errores, todos con la posicion (base 1) del caracter donde se detecto el problema:

1. **Errores Lexicos** (`LexicalException`) — Caracteres no reconocidos en la entrada.

   ```
   $ dotnet run --project CompiladorAlgebraico -- "2 & 3"
   Lex Analyzer Error (posicion 3): caracter no reconocido '&'
   ```

2. **Errores Sintacticos** (`SyntacticException`) — Expresiones mal formadas: operadores sin operando, parentesis sin cerrar, tokens sobrantes.

   ```
   $ dotnet run --project CompiladorAlgebraico -- "2 +"
   Syntactic Analyzer Error (posicion 4): se esperaba un numero o '(' pero se encontro el fin de la expresion
   ```

3. **Errores de Evaluacion** (`EvaluationException`) — Expresiones bien formadas que no se pueden evaluar: division por cero, literales fuera del rango de `double`.

   ```
   $ dotnet run --project CompiladorAlgebraico -- "5 / (3 - 3)"
   Evaluation Error (posicion 3): division por cero
   ```

Las tres derivan de `CompilerException`, que expone `Kind`, `Detail` y `Position`, de modo que un
programa que use el parser como libreria puede capturarlas todas a la vez o distinguirlas por tipo.

## Tests

El proyecto incluye una suite de tests en xUnit que cubre la gramatica, la precedencia, la
asociatividad, los decimales y cada tipo de error:

```bash
dotnet test
```

Los tests se ejecutan automaticamente en GitHub Actions en cada push y pull request
(ver `.github/workflows/ci.yml`).

## Tecnologias

- **Lenguaje**: C#
- **Framework**: .NET 8.0 (LTS)
- **Tests**: xUnit
- **Paradigma**: Analisis descendente recursivo

## Posibles Mejoras

- [ ] Operaciones adicionales (potencia, modulo)
- [ ] Funciones matematicas (`sin`, `cos`, `tan`, `sqrt`)
- [ ] Variables y asignaciones
- [ ] Modo interactivo (REPL) que evalue varias expresiones sin reiniciar
- [ ] Notacion cientifica en los literales (`1e10`)
- [ ] Generar un AST en vez de evaluar durante el analisis, para permitir optimizaciones

## Contribuciones

Las contribuciones son bienvenidas. Para contribuir:

1. Haz un fork del repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-caracteristica`)
3. Realiza tus cambios y commitea (`git commit -m 'Agregar nueva caracteristica'`)
4. Asegurate de que `dotnet test` pase
5. Sube los cambios (`git push origin feature/nueva-caracteristica`)
6. Abre un Pull Request

## Licencia

Este proyecto aun no tiene una licencia asignada. Sin un archivo `LICENSE`, se aplican los
derechos de autor por defecto: el codigo se puede ver, pero no reutilizar. Si quieres que sea
software libre, agrega un archivo `LICENSE` (por ejemplo [MIT](https://choosealicense.com/licenses/mit/)).
