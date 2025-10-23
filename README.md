# 🧬 Conway’s Game of Life — C# / WPF

A visual and interactive implementation of **Conway’s Game of Life**, built in **C# (.NET 8)** using **WPF** for the user interface.  
You can toggle cells to set the initial pattern, then start the simulation to watch how life evolves!

---

## 🧩 What is the Game of Life?

The **Game of Life** is a **cellular automaton** created by mathematician **John Horton Conway** in 1970.  
It’s not a traditional game — there are no players.  
Instead, it’s a zero-player simulation that evolves based on simple rules applied to a grid of cells.

Each cell can be in one of two states:
- **Alive**
- **Dead**

The board evolves through discrete time steps (“generations”).  
At each step, the next state of every cell depends on the current state of its eight neighbors.

---

## 🔣 Rules of the Game

1. **Underpopulation**  
   - A live cell with **fewer than two** live neighbors **dies**.

2. **Survival**  
   - A live cell with **two or three** live neighbors **stays alive**.

3. **Overpopulation**  
   - A live cell with **more than three** live neighbors **dies**.

4. **Reproduction**  
   - A dead cell with **exactly three** live neighbors **becomes alive**.

These simple rules lead to surprisingly complex and beautiful patterns — oscillators, gliders, and stable structures.

---

## 🖥️ Features

✅ Interactive grid — click cells to toggle between **alive** and **dead**  
✅ Adjustable simulation speed  
✅ Start / Pause / Step controls  
✅ Randomize and Clear options  
✅ Simple, performant WPF rendering  
✅ Modular design — simulation logic separated from UI in `Life.Core`

