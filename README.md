# Compilador Algebraico

Un compilador/interprete de expresiones algebraicas escrito en C# que evalua operaciones matematicas utilizando analisis lexico y sintactico.

## Descripcion

Este proyecto implementa un compilador algebraico que analiza y evalua expresiones matematicas. Utiliza tecnicas de compiladores como analisis lexico (scanner) y analisis sintactico descendente recursivo (parser) para procesar las expresiones.

## Caracteristicas

- Evaluacion de expresiones matematicas
- Operaciones soportadas: suma (`+`), resta (`-`), multiplicacion (`*`) y division (`/`)
- Soporte para numeros enteros
- Manejo de parentesis para agrupar operaciones
- Soporte para operador unario negativo (e.g. `-5 + 10`)
- Analisis sintactico descendente recursivo
- Aritmetica de doble precision (`double`) en los calculos internos
- Deteccion de errores lexicos y sintacticos

## Requisitos

- [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1) o superior
- Sistema operativo compatible con .NET Core (Windows, Linux, macOS)

## Instalacion

1. Clona el repositorio:

```bash
git clone https://github.com/esaban17/compiladoralgebraico.git
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

Al ejecutar el programa, se solicita ingresar una expresion matematica. El programa la evalua y muestra el resultado.

### Ejemplos de expresiones validas

```
2 + 3 * 4
(5 + 3) * 2
10 - 2 * 3
100 / (2 + 3)
-5 + 10
(2 + 3) * (4 - 1)
```

### Ejemplo de ejecucion

```
Ingrese la expresion:
> 2 + 3 * 4

----------------------------
El resultado es: 14
----------------------------
```

## Estructura del Proyecto

```
CompiladorAlgebraico/
├── CompiladorAlgebraico.sln
├── README.md
└── CompiladorAlgebraico/
    ├── CompiladorAlgebraico.csproj
    ├── Program.cs
    ├── Scanner.cs
    ├── Parser.cs
    ├── Token.cs
    └── TokenType.cs
```

### Componentes

| Archivo | Descripcion |
|---------|-------------|
| **Program.cs** | Punto de entrada. Lee la expresion del usuario e invoca al parser para evaluarla. |
| **Scanner.cs** | Analizador lexico que convierte la cadena de entrada en tokens. Reconoce numeros enteros, operadores y parentesis. Detecta errores lexicos. |
| **Parser.cs** | Analizador sintactico descendente recursivo. Evalua las expresiones respetando la precedencia de operadores mediante las funciones `E()`, `EPrime()`, `T()`, `TPrime()`, `F()` y `R()`. |
| **Token.cs** | Clase que representa un token con su tipo (`Tag`) y valor (`Value`). |
| **TokenType.cs** | Enumeracion de tipos de tokens soportados. |

### Tipos de Tokens

| Token | Simbolo | Descripcion |
|-------|---------|-------------|
| `Number` | `0-9` | Numeros enteros |
| `Plus` | `+` | Suma |
| `Minus` | `-` | Resta |
| `Mult` | `*` | Multiplicacion |
| `Div` | `/` | Division |
| `LParen` | `(` | Parentesis izquierdo |
| `RParen` | `)` | Parentesis derecho |
| `EOF` | — | Fin de entrada |

## Gramatica

El parser implementa la siguiente gramatica libre de contexto que respeta la precedencia de operadores:

```
E  -> T E'
E' -> + T E' | - T E' | e
T  -> F T'
T' -> * F T' | / F T' | e
F  -> - R | R
R  -> number | ( E )
```

Donde:

- **E**: Expresion (maneja suma y resta)
- **T**: Termino (maneja multiplicacion y division)
- **F**: Factor (maneja operador unario negativo)
- **R**: Elemento basico (numeros o expresiones entre parentesis)
- **e**: Epsilon (cadena vacia)

## Manejo de Errores

El compilador detecta y reporta dos tipos de errores:

1. **Errores Lexicos** — Caracteres no reconocidos en la entrada.
   - Mensaje: `Lex Analyzer Error`

2. **Errores Sintacticos** — Expresiones mal formadas (e.g. operadores consecutivos, parentesis sin cerrar).
   - Mensaje: `Syntactic Analyzer Error`

## Tecnologias

- **Lenguaje**: C#
- **Framework**: .NET Core 3.1
- **Paradigma**: Analisis descendente recursivo

## Posibles Mejoras

- [ ] Soporte para numeros decimales (e.g. `3.14`)
- [ ] Operaciones adicionales (potencia, modulo)
- [ ] Funciones matematicas (`sin`, `cos`, `tan`, `sqrt`)
- [ ] Variables y asignaciones
- [ ] Modo interactivo (REPL)
- [ ] Mensajes de error mas descriptivos con posicion del error
- [ ] Tests unitarios

## Contribuciones

Las contribuciones son bienvenidas. Para contribuir:

1. Haz un fork del repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-caracteristica`)
3. Realiza tus cambios y commitea (`git commit -m 'Agregar nueva caracteristica'`)
4. Sube los cambios (`git push origin feature/nueva-caracteristica`)
5. Abre un Pull Request

## Licencia

Este proyecto es de codigo abierto y esta disponible bajo los terminos que el autor desee especificar.
