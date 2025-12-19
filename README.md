# Compilador Algebraico

Un compilador/intérprete de expresiones algebraicas escrito en C# que evalúa operaciones matemáticas utilizando análisis léxico y sintáctico.

## Descripción

Este proyecto implementa un compilador algebraico que puede analizar y evaluar expresiones matemáticas. Utiliza técnicas de compiladores como análisis léxico (scanner) y análisis sintáctico descendente recursivo (parser) para procesar las expresiones.

## Características

- ✨ Evaluación de expresiones matemáticas
- ➕ Operaciones soportadas: suma, resta, multiplicación y división
- 🔢 Soporte para números enteros y decimales
- 📐 Manejo de paréntesis para agrupar operaciones
- ⚡ Análisis sintáctico descendente recursivo
- 🛡️ Detección de errores léxicos y sintácticos

## Requisitos

- .NET Core 3.1 o superior
- Sistema operativo compatible con .NET Core (Windows, Linux, macOS)

## Instalación

1. Clona el repositorio:
```bash
git clone <url-del-repositorio>
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

Al ejecutar el programa, se te solicitará ingresar una expresión matemática. El programa la evaluará y mostrará el resultado.

### Ejemplos de expresiones válidas:

```
2 + 3 * 4
(5 + 3) * 2
10 - 2 * 3
100 / (2 + 3)
-5 + 10
(2 + 3) * (4 - 1)
```

### Ejemplo de ejecución:

```
> 2 + 3 * 4

----------------------------
El resultado es: 14
----------------------------
```

## Estructura del Proyecto

El proyecto está organizado en los siguientes componentes:

### `Program.cs`
Punto de entrada de la aplicación. Lee la expresión del usuario e invoca al parser para evaluarla.

### `Scanner.cs`
Analizador léxico (lexer) que:
- Convierte la cadena de entrada en tokens
- Reconoce números, operadores y paréntesis
- Maneja espacios en blanco
- Detecta errores léxicos

### `Parser.cs`
Analizador sintáctico que:
- Implementa una gramática formal usando análisis descendente recursivo
- Evalúa las expresiones respetando la precedencia de operadores
- Funciones gramaticales: `E()`, `EPrime()`, `T()`, `TPrime()`, `F()`, `R()`
- Detecta errores sintácticos

### `Token.cs`
Representa un token con su tipo y valor.

### `TokenType.cs`
Enumeración de los tipos de tokens soportados:
- `Number`: Números
- `Plus`: Operador suma (+)
- `Minus`: Operador resta (-)
- `Mult`: Operador multiplicación (*)
- `Div`: Operador división (/)
- `LParen`: Paréntesis izquierdo (()
- `RParen`: Paréntesis derecho ())
- `EOF`: Fin de archivo

## Gramática

El parser implementa la siguiente gramática que respeta la precedencia de operadores:

```
E  → T E'
E' → + T E' | - T E' | ε
T  → F T'
T' → * F T' | / F T' | ε
F  → - R | R
R  → number | ( E )
```

Donde:
- **E**: Expresión (maneja suma y resta)
- **T**: Término (maneja multiplicación y división)
- **F**: Factor (maneja operadores unarios)
- **R**: Elemento básico (números o expresiones entre paréntesis)

## Manejo de Errores

El compilador detecta y reporta dos tipos de errores:

1. **Errores Léxicos**: Caracteres no reconocidos en la entrada
   - Mensaje: "Lex Analyzer Error"

2. **Errores Sintácticos**: Expresiones mal formadas
   - Mensaje: "Syntactic Analyzer Error"

## Tecnologías Utilizadas

- **Lenguaje**: C#
- **Framework**: .NET Core 3.1
- **Paradigma**: Análisis descendente recursivo

## Contribuciones

Las contribuciones son bienvenidas. Si deseas mejorar el proyecto:

1. Haz un fork del repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-caracteristica`)
3. Realiza tus cambios y commitea (`git commit -am 'Agregar nueva característica'`)
4. Sube los cambios (`git push origin feature/nueva-caracteristica`)
5. Abre un Pull Request

## Posibles Mejoras

- [ ] Soporte para números decimales
- [ ] Operaciones adicionales (potencia, módulo, etc.)
- [ ] Funciones matemáticas (sin, cos, tan, sqrt, etc.)
- [ ] Variables y asignaciones
- [ ] Modo interactivo (REPL)
- [ ] Mensajes de error más descriptivos

## Licencia

Este proyecto es de código abierto y está disponible bajo los términos que el autor desee especificar.

## Autor

Proyecto desarrollado como parte del aprendizaje de técnicas de compiladores y análisis sintáctico.
