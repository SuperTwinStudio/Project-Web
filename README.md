# *< Project Paper >*
---
Juego de acción y exploración de mazmorras donde deberás ayudar a un pequeño origami a encontrar a su hermano en un vasto mundo fantástico. 

Trabajo desarrollado para la asignatura *Juegos para Web* del grado *Diseño y desarrollo de videojuegos* de la URJC.
## Twin Studio  
---
- **Portfolio:**
- **Linktree**
- **Youtube:** 
- **X (Twitter):**
## Equipo de desarrollo
---
- Alejandro Paniagua Moreno (@BOTPanzer) -> a.paniagua.2022@alumnos.urjc.es
- Raúl Alfonso Pérez (@racurrandom) -> r.alfonso.2022@alumnos.urjc.es
- Pablo Quiñones Gonzalez (@ThatBit345) -> p.quinones.2022@alumnos.urjc.es
- Sara María Romero Bermejo (@SaraRomBer) -> sm.romero.2022@alumnos.urjc.es
-  María Marquez García (@martesytrece) -> m.marquezg.2022@alumnos.urjc.es
- Carlos Vega San Román (@CarlosVSR) -> 

# GDD
---
**Versión 0.5**
## Historial de versiones
---
Historial de versiones y cambios realizados en el documento.
- **Versión 0.1 - 27/09/25:** esquema inicial, formato y borrador de documento. Adición de los apartados del concepto del juego, el Game Flow, Enemigos y jefes, y Tesoros.
- **Versión 0.2 - 29/09/25:** desarrollo del apartado introductorio (Concepto, Género y Público Objetivo), adición del apartado Temática en la sección de Arte, expansión de la sección Mecánicas (tutorial, tienda, mazmorras y efectos de estado). Breve desarrollo de la narrativa.
- **Versión 0.3 - 1/10/25:** revisión general de la estructura y contenido del documento. Adición de la sección Monetización y planificación. Reestructuración de sección Mecánicas (Controles del jugador, Clasificación de armas, Mejoras y tipos de tesoros). Breve desarrollo de la sección Level Design y desarrollo completo de Lore y Narrativa. 
- **Versión 0.4 - 6/10/25:** desarrollo del apartado Audio y SFX. Desarrollo del apartado Interfaces. Balanceo de las clases de armas.
- **Versión 0.5 - 11/10/25:** segunda revisión y cambios menores en todo el documento. Desarrollo del apartado Generación procedimental de mazmorras. Refinado del apartado Interfaces, Diagrama de flujo y Mejoras.

## Introducción
---
### Concepto
Project Paper es un videojuego de acción rogue-lite en perspectiva isométrica ambientado en un reino fantástico en miniatura. El juego está centrado en el combate y en la exploración de mazmorras generadas procedimentalmente, siguiendo un ciclo de meta-progresión.

El juego sigue la historia de Washi, un pequeño origami que se embarca en una misión para rescatar a su gemelo Kami de las garras del Culto de la Llama. Durante su viaje deberá abrirse paso a través de las mazmorras hasta alcanzar y derrotar a cada uno de los miembros que forman la secta, y así recuperar los papiros que su hermano dejó atrás para truncar los malvados planes del culto.

## Género
Project Paper combina los géneros acción y rogue-lite. Algunas características destacables de estos géneros presentes en el juego:
- **Acción:** el juego es dinámico, requiere cierto grado de velocidad y destreza para despejar los mapas y derrotar a los jefes.
- **Rogue-lite:** el juego cuenta con un sistema de mazmorras generadas procedimentalmente, y cada “run” es parcialmente aleatoria. La muerte en partida es permanente, pero hay ciertos elementos, como mejoras o armas que se conservan entre partidas para facilitar el avance del jugador (meta-progresión).  

## Propósito, Público Objetivo y Plataformas
El propósito de este videojuego es crear un ciclo de juego dinámico en el que la sensación de progresión sea fluida y satisfactoria, además de ofrecer una experiencia visualmente atractiva. Se busca que los jugadores puedan disfrutar del juego independientemente de la duración de la sesión, y que mantengan el interés en completarlo. 

Se identifican dos tipos de público objetivo para este juego:
- **Jugadores experimentados:** al igual que el resto de juegos rogue-lite,  el público diana son jóvenes de entre 16 a 35 años que disfruten de tanto acción rápida como progresión gradual, y que ya se encuentren familiarizados con títulos análogos como Enter The Gungeon, Hades, o The Binding of Isaac.
- **Nuevos jugadores:** personas jóvenes que no hayan probado videojuegos del género con anterioridad pero que estén interesadas en probar algo novedoso, conocer la historia del juego o simplemente disfrutar de su estética.

Project Paper está pensado para ejecutarse en navegadores de ordenadores a través de [itch.io](http://itch.io) y en móviles, con posibilidad de ampliar a otros mercados de la plataforma PC como Steam o Epic Store.
## Monetización y planificación
---
### Modelo de monetización
La versión Release del juego se lanzará en [itch.io](http://itch.io) y será free-to-play, con opción de donación a través de la plataforma. En función del grado de éxito en el desarrollo y el impacto mediático en redes sociales, se plantean más otras opciones de monetización:
#### Opción 1 - Kickstarter
Se plantea la posibilidad de lanzar la versión Release como una demo y diseñar un Kickstarter para una versión del juego más grande y pulida, para posteriormente publicar el juego como título de pago único en Steam. A continuación se plantea una tabla orientativa de Stretch Goals y recompensas por tiers para incentivar a los potenciales mecenas.

Para el cálculo aproximado de Stretch goals se han tomado juegos como Hollow Knight o Shovel Knight.

Para el apartado de tiers también se han tomado como referencia Hollow Knight y Shovel Knight, y se listan las recompensas en función de la cantidad donada por mecenas. Las recompensas son acumulativas, para cada tier se suman las recompensas de la propia tier y las de las anteriores. Se han diseñado priorizando en contenido digital para minimizar gastos del equipo.

#### Opción 2 - Publishers
Se plantea la posibilidad de ofrecer el juego en ferias indie a nivel nacional y/o internacional, buscando la financiación de publishers. Algunos ejemplos de ferias a considerar podrían ser la TLP Tenerife, BIG e IndieDevDay a nivel nacional o la Gamescom de Colonia a nivel internacional.

En el supuesto de lograr el apoyo de un publisher se consideraría desplegar soporte a otras plataformas como consolas (Switch, Switch 2). El listado de precios dependería de la editorial, pero se enumeran ejemplos orientativos de precios para los productos, tanto el videojuego como merchandising:

*En el supuesto de lograr el apoyo de un publisher se consideraría desplegar soporte a otras plataformas como consolas (Switch, Switch 2). El listado de precios dependería de la editorial, pero se enumeran ejemplos orientativos de precios para los productos, tanto el videojuego como merchandising:*

