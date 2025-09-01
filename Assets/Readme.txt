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

Juan Alfredo Ju�rez Tamayo

ESPA�OL
Descripci�n general

ThecnicalProjectJAJT es un juego tipo slot machine en su versi�n m�s b�sica. Incluye todo el material gr�fico necesario para probar el funcionamiento.

C�mo funciona internamente

El juego utiliza Unity y est� programado en C#.

Los rodillos giran y al detenerse el sistema eval�a combinaciones ganadoras.

Se definen varios patrones de s�mbolos (lineales y wild) para evaluar las combinaciones ganadoras.

Los patrones wild se eval�an primero. Si un s�mbolo gana con un patr�n wild, no puede volver a ganar en un patr�n lineal.

Se usan listas de s�mbolos y objetos activos para manejar los patrones.

El sistema activa indicadores en los s�mbolos que forman combinaciones ganadoras.

La interfaz muestra el premio ganado o un mensaje vac�o si no hay ganancia.

Hay manejo de errores con try/catch para evitar fallos en tiempo de ejecuci�n.

C�mo jugar

El jugador solo debe presionar el bot�n Spin.

Los rodillos girar�n y se detendr�n autom�ticamente.

Si hay combinaciones ganadoras, se mostrar� el premio en pantalla.


Estructura del c�digo y explicaci�n breve

PatternManager: Gestiona los s�mbolos, los rodillos activos, eval�a los patrones (dando prioridad a Wild sobre Lineal) y actualiza el UI con el premio.

BasicLinearPattern y WildPattern: Definen reglas para evaluar combinaciones lineales y wild respectivamente.

SpinManager: Controla el inicio, la pausa y la detenci�n de los rodillos y gestiona el flujo del juego.

EnableButton: Habilita y deshabilita el bot�n Spin seg�n el estado del juego.

Pattern: Clase serializable para definir �ndices de patrones.

BaseSymbolPattern: Clase abstracta que provee la l�gica para evaluar s�mbolos y calcular premios.

UniqueList: Almacena s�mbolos �nicos, utilizados para manejar resultados posibles y validaciones.

RollerCylinder: Representa la l�gica de rotaci�n del rodillo y la actualizaci�n de s�mbolos visibles.

SlotState Enum: Define el estado actual de la m�quina:

Idle  Esperando acci�n

Starting  Inicializando carretes

Spinning  Carretes girando

Stopping  Carretes deteni�ndose

FillingList  Recolectando s�mbolos activos

Evaluating  Verificando patrones ganadores

Requisitos

Unity 2022.3.40f1 o superior (se recomienda 2021+)

C# 8.0 o superior

TMP (TextMeshPro) importado para mostrar textos UI

Autor / Cr�ditos

Juan Alfredo Ju�rez Tamayo