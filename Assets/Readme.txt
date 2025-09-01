ThecnicalTestJAJT

=================

ENGLISH
Overview

ThecnicalProjectJAJT is a basic version of a slot machine game. All graphic assets are included for testing purposes.

How it works internally

The game is made with Unity and coded in C#.

Reels spin and stop automatically; the system then evaluates winning combinations.

Multiple patterns (linear and wild) are defined to check winning lines.

Wild patterns are evaluated first. If a symbol wins with a Wild pattern, it cannot be rewarded again in a Linear pattern.

Symbol lists and active reel objects manage the pattern evaluations.

Indicators activate on symbols that form winning combinations.

The UI displays the reward or clears the message if no win.

Error handling with try/catch is implemented to prevent runtime crashes.


How to play

The player just presses the Spin button.

The reels spin and stop automatically.

If winning combinations appear, the reward is displayed on screen.



Code structure and brief explanation

PatternManager: Manages symbols, active reels, evaluates patterns (prioritizing Wild over Linear), and updates the UI with rewards.

BasicLinearPattern and WildPattern: Define rules for evaluating linear and wild symbol combinations.

SpinManager: Controls starting, pausing, and stopping reels, managing the game flow.

EnableButton: Enables/disables the Spin button based on the game state.

Pattern: Serializable class for defining pattern index sequences.

BaseSymbolPattern: Abstract class providing core logic for symbol evaluation and reward calculation.

UniqueList: Stores unique symbols, used to manage possible outcomes and checks.

RollerCylinder: Represents the reel's rotation logic and visible symbol updates.

SlotState Enum: Defines the current state of the slot machine and reels:

Idle  Waiting for input

Starting  Initializing reels

Spinning  Reels are spinning

Stopping  Reels are stopping

FillingList  Collecting active symbols

Evaluating  Checking for winning patterns

Requirements

Unity 2022.3.40f1 or newer (2021+ recommended)

C# 8.0 or newer

TextMeshPro package imported for UI text

Author / Credits

Juan Alfredo Juárez Tamayo

ESPAÑOL
Descripción general

ThecnicalProjectJAJT es un juego tipo slot machine en su versión más básica. Incluye todo el material gráfico necesario para probar el funcionamiento.

Cómo funciona internamente

El juego utiliza Unity y está programado en C#.

Los rodillos giran y al detenerse el sistema evalúa combinaciones ganadoras.

Se definen varios patrones de símbolos (lineales y wild) para evaluar las combinaciones ganadoras.

Los patrones wild se evalúan primero. Si un símbolo gana con un patrón wild, no puede volver a ganar en un patrón lineal.

Se usan listas de símbolos y objetos activos para manejar los patrones.

El sistema activa indicadores en los símbolos que forman combinaciones ganadoras.

La interfaz muestra el premio ganado o un mensaje vacío si no hay ganancia.

Hay manejo de errores con try/catch para evitar fallos en tiempo de ejecución.

Cómo jugar

El jugador solo debe presionar el botón Spin.

Los rodillos girarán y se detendrán automáticamente.

Si hay combinaciones ganadoras, se mostrará el premio en pantalla.


Estructura del código y explicación breve

PatternManager: Gestiona los símbolos, los rodillos activos, evalúa los patrones (dando prioridad a Wild sobre Lineal) y actualiza el UI con el premio.

BasicLinearPattern y WildPattern: Definen reglas para evaluar combinaciones lineales y wild respectivamente.

SpinManager: Controla el inicio, la pausa y la detención de los rodillos y gestiona el flujo del juego.

EnableButton: Habilita y deshabilita el botón Spin según el estado del juego.

Pattern: Clase serializable para definir índices de patrones.

BaseSymbolPattern: Clase abstracta que provee la lógica para evaluar símbolos y calcular premios.

UniqueList: Almacena símbolos únicos, utilizados para manejar resultados posibles y validaciones.

RollerCylinder: Representa la lógica de rotación del rodillo y la actualización de símbolos visibles.

SlotState Enum: Define el estado actual de la máquina:

Idle  Esperando acción

Starting  Inicializando carretes

Spinning  Carretes girando

Stopping  Carretes deteniéndose

FillingList  Recolectando símbolos activos

Evaluating  Verificando patrones ganadores

Requisitos

Unity 2022.3.40f1 o superior (se recomienda 2021+)

C# 8.0 o superior

TMP (TextMeshPro) importado para mostrar textos UI

Autor / Créditos

Juan Alfredo Juárez Tamayo