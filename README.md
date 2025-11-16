<img width="512" height="512" alt="Paper_Star_Pile" src="https://github.com/user-attachments/assets/9a0bdc5e-e762-4860-868b-4c54eec5c6aa" /># Washi's Adventure

Juego de acción y exploración de mazmorras donde deberás ayudar a un pequeño origami a encontrar a su hermano en un vasto mundo fantástico. 

Trabajo desarrollado para la asignatura de ***Juegos para Web*** del grado de ***Diseño y Desarrollo de Videojuegos*** de la URJC.



# Twin Studio

## Redes Sociales

- [Portfolio](http://supertwinstudio.github.io/Portfolio/)
- [Itch.io](https://supertwinstudio.itch.io/)
- [Instagram](https://www.instagram.com/supertwinstudio/)
- [X (Twitter)](https://x.com/superTwinStudio)
- [Youtube](https://www.youtube.com/@superTwinStudio)
- [Linktree](https://linktr.ee/supertwinstudio)

## Desarrolladores

- Alejandro Paniagua Moreno  
  - [@BOTPanzer](https://github.com/BOTPanzer)  
  - a.paniagua.2022@alumnos.urjc.es
- Raúl Alfonso Pérez  
  - [@racurrandom](https://github.com/racurrandom)  
  - r.alfonso.2022@alumnos.urjc.es
- Pablo Quiñones Gonzalez  
  - [@ThatBit345](https://github.com/ThatBit345)  
  - p.quinones.2022@alumnos.urjc.es
- Sara María Romero Bermejo  
  - [@SaraRomBer](https://github.com/SaraRomBer)  
  - sm.romero.2022@alumnos.urjc.es
- María Marquez García  
  - [@martesytrece](https://github.com/martesytrece)  
  - m.marquezg.2022@alumnos.urjc.es
- Carlos Vega San Román  
  - [@CarlosVSR](https://github.com/CarlosVSR)



# GDD

## Índice

1. [Historial de Versiones](#1-historial-de-versiones)
2. [Introducción](#2-introducción)
3. [Monetización & Planificación](#3-monetización--planificación)
4. [Trasfondo](#4-trasfondo)
5. [Game Flow](#5-game-flow)
6. [Mecánicas](#6-mecánicas)
7. [Level Design](#7-level-design)
8. [Enemigos & Bosses](#8-enemigos--bosses)
9. [Arte](#9-arte)
10. [Audio](#10-audio)
11. [Interfaces](#11-interfaces)



## 1. Historial de Versiones

**Versión 0.5**

Historial de versiones y cambios realizados en el documento.
- **Versión 0.1 - 27/09/25:** esquema inicial, formato y borrador de documento. Adición de los apartados del concepto del juego, el Game Flow, Enemigos y jefes, y Tesoros.
- **Versión 0.2 - 29/09/25:** desarrollo del apartado introductorio (Concepto, Género y Público Objetivo), adición del apartado Temática en la sección de Arte, expansión de la sección Mecánicas (tutorial, tienda, mazmorras y efectos de estado). Breve desarrollo de la narrativa.
- **Versión 0.3 - 1/10/25:** revisión general de la estructura y contenido del documento. Adición de la sección Monetización y planificación. Reestructuración de sección Mecánicas (Controles del jugador, Clasificación de armas, Mejoras y tipos de tesoros). Breve desarrollo de la sección Level Design y desarrollo completo de Lore y Narrativa. 
- **Versión 0.4 - 6/10/25:** desarrollo del apartado Audio y SFX. Desarrollo del apartado Interfaces. Balanceo de las clases de armas.
- **Versión 0.5 - 11/10/25:** segunda revisión y cambios menores en todo el documento. Desarrollo del apartado Generación procedimental de mazmorras. Refinado del apartado Interfaces, Diagrama de flujo y Mejoras.
- **Versión 0.6 - 15/11/25:** ampliación del documento con el contenido de la beta, enfatizando en el apartado de las mecánicas:
	- **Jugador:** adición de la habilidad *Dash* al moveset y el botón de pausado.
		- Ajustes en el feedback del jugador.
	- **Armas:** cambios en las descripciones de las clases de armas y ajustes en las habilidades y propiedades de las clases Lanza y Espada. Exclusión de la clase Mental por la complejidad de desarrollo.
	- **Mejoras permanentes:** cambios y adiciones (Purpurina) en las mejoras permanentes.
	- **Objetos:** diseño y desarrollo de una categoría de objetos para mejorar el Game Flow.
	- **Enemigos:** reajuste en el nivel de dificultad de los enemigos. Cambios en el moveset del jefe Miso Beast y exclusión del jefe Calamarcillo.



## 2. Introducción

### Concepto

Project Paper es un videojuego de acción rogue-lite en perspectiva isométrica ambientado en un reino fantástico en miniatura. El juego está centrado en el combate y en la exploración de mazmorras generadas procedimentalmente, siguiendo un ciclo de meta-progresión.

El juego sigue la historia de Washi, un pequeño origami que se embarca en una misión para rescatar a su gemelo Kami de las garras del Culto de la Llama. Durante su viaje deberá abrirse paso a través de las mazmorras hasta alcanzar y derrotar a cada uno de los miembros que forman la secta, y así recuperar los papiros que su hermano dejó atrás para truncar los malvados planes del culto.

### Género

Project Paper combina los géneros acción y rogue-lite. Algunas características destacables de estos géneros presentes en el juego:
- **Acción:** el juego es dinámico, requiere cierto grado de velocidad y destreza para despejar los mapas y derrotar a los jefes.
- **Rogue-lite:** el juego cuenta con un sistema de mazmorras generadas procedimentalmente, y cada “run” es parcialmente aleatoria. La muerte en partida es permanente, pero hay ciertos elementos, como mejoras o armas que se conservan entre partidas para facilitar el avance del jugador (meta-progresión).  

### Propósito, Público Objetivo y Plataformas

El propósito de este videojuego es crear un ciclo de juego dinámico en el que la sensación de progresión sea fluida y satisfactoria, además de ofrecer una experiencia visualmente atractiva. Se busca que los jugadores puedan disfrutar del juego independientemente de la duración de la sesión, y que mantengan el interés en completarlo. 

Se identifican dos tipos de público objetivo para este juego:
- **Jugadores experimentados:** al igual que el resto de juegos rogue-lite,  el público diana son jóvenes de entre 16 a 35 años que disfruten de tanto acción rápida como progresión gradual, y que ya se encuentren familiarizados con títulos análogos como Enter The Gungeon, Hades, o The Binding of Isaac.
- **Nuevos jugadores:** personas jóvenes que no hayan probado videojuegos del género con anterioridad pero que estén interesadas en probar algo novedoso, conocer la historia del juego o simplemente disfrutar de su estética.

Project Paper está pensado para ejecutarse en navegadores de ordenadores a través de [itch.io](https://supertwinstudio.itch.io/project-paper) y en móviles, con posibilidad de ampliar a otros mercados de la plataforma PC como Steam o Epic Store.



## 3. Monetización & Planificación

### Modelo de monetización

La versión Release del juego se lanzará en [itch.io](https://supertwinstudio.itch.io/project-paper) y será free-to-play, con opción de donación a través de la plataforma. En función del grado de éxito en el desarrollo y el impacto mediático en redes sociales, se plantean más otras opciones de monetización:

#### Opción 1 - Kickstarter

Se plantea la posibilidad de lanzar la versión Release como una demo y diseñar un Kickstarter para una versión del juego más grande y pulida, para posteriormente publicar el juego como título de pago único en Steam. A continuación se plantea una tabla orientativa de Stretch Goals y recompensas por tiers para incentivar a los potenciales mecenas.

Para el cálculo aproximado de Stretch goals se han tomado juegos como Hollow Knight o Shovel Knight.

| **Cuantity** | **Stretch Goal**                                                                                                   |
| ------------ | ------------------------------------------------------------------------------------------------------------------ |
| 30.000€      | Goal mínimo para comenzar el desarrollo, suponiendo que todos los integrantes del grupo participen en el proyecto. |
| 35.000€      | Adición de una nueva zona                                                                                          |
| 40.000€      | Adición de un segundo personaje jugable                                                                            |
| 60.000€      | Adición del modo cooperativo                                                                                       |

Para el apartado de tiers también se han tomado como referencia Hollow Knight y Shovel Knight, y se listan las recompensas en función de la cantidad donada por mecenas. Las recompensas son acumulativas, para cada tier se suman las recompensas de la propia tier y las de las anteriores. Se han diseñado priorizando en contenido digital para minimizar gastos del equipo.

| **Price** | **Tier**                                                                                                |
| --------- | ------------------------------------------------------------------------------------------------------- |
| 10€       | Copia digital del juego para PC                                                                         |
| 15€       | PDF del libro de instrucciones                                                                          |
| 25€       | PDF del libro de arte                                                                                   |
| 45€       | Banda sonora en digital                                                                                 |
| 75€       | Acceso a la beta + 2 copias digitales del juego (3 en total)                                            |
| 150€      | Deja tu propio mensaje dentro del juego                                                                 |
| 200€      | Dibujo original en acuarela firmado por el grupo + 2 copias digitales del juego (5 en total)            |
| 400€      | Aparece como NPC dentro del juego + 2 copias digitales del juego (7 en total) + libro de arte en físico |
| 600€      | Diseña un jefe secreto + 3 copias digitales del juego (10 en total)                                     |

#### Opción 2 - Publishers

Se plantea la posibilidad de ofrecer el juego en ferias indie a nivel nacional y/o internacional, buscando la financiación de publishers. Algunos ejemplos de ferias a considerar podrían ser la TLP Tenerife, BIG e IndieDevDay a nivel nacional o la Gamescom de Colonia a nivel internacional.

En el supuesto de lograr el apoyo de un publisher se consideraría desplegar soporte a otras plataformas como consolas (Switch, Switch 2). El listado de precios dependería de la editorial, pero se enumeran ejemplos orientativos de precios para los productos, tanto el videojuego como merchandising:

| **Price** | **Product**                                                                         |
| --------- | ----------------------------------------------------------------------------------- |
| 13.99€    | Copia digital del juego en Steam, Humble Bundle o en Switch/ Switch 2               |
| 5.99€     | Banda sonora en digital en Steam (válida para combinar en packs rebajados de Steam) |
| 25.99€    | Copia en físico del juego para Switch/ Switch 2                                     |
| 40€       | Libro de arte en físico (tapa dura)                                                 |
| 20€       | Figura/s promocional/es de personajes relevantes del juego                          |
| 4.99€     | Póster promocional                                                                  |

###  Planificación

#### Equipo de desarrollo

El proyecto cuenta con un equipo de desarrollo polivalente, por lo que todos los miembros están capacitados para participar en todas las áreas del proyecto. El equipo funcionará de forma autogestionada utilizando un formato de trabajo similar al de las metodologías ágiles y escogiendo tareas en función de las áreas en las que destacan. El tracking de tareas se realizará mediante Github Projects, para garantizar la comunicación y evitar solapamientos. A continuación se muestra una tabla con los roles principales que desempeña cada miembro del equipo.

#### Estimación de desarrollo

Por la naturaleza del proyecto y el tiempo asignado se espera trabajar en un formato parecido al de los sprints de Scrum (de dos a cuatro semanas por versión lanzada). 

- **Alpha - 14/10/25:** se espera cubrir toda la parte de las mecánicas del juego e iniciar el despliegue de RRSS para dar visibilidad al proyecto. 
- **Beta - 16/11/25:** implementación de mecanicas restantes, corrección de bugs importantes, cobertura casi completa de la parte artística, testeo intensivo.  
- **Release - (fecha por determinar):** corrección de bugs identificados durante la fase de testeo, pulido de mecánicas, arte y game feel.



## 4. Trasfondo

### Lore

En el trasfondo se narran los eventos previos a los eventos del juego y explican el arranque de la trama. 

La historia se desarrolla en el interior de una habitación, en un escritorio genérico de estudiante que está encantado. Todos los trozos de papel fueron alcanzados por un poder sagrado y cobraron vida. Así comenzaron una existencia pacífica sobre el tablero de la mesa, formando un reino mágico gobernado por un rey. Sin embargo, no todos los origamis se encontraban a favor de vivir bajo su gobierno, por lo que descendieron a las profundidades de los cajones del escritorio y desaparecieron durante un tiempo. Sin embargo, la oscuridad corrompió sus corazones, y cuando un día hallaron un poder malvado en el cajón más profundo, no dudaron en liberarlo y enviarlo contra el reino. Era Akarigami, deidad destructora del papel, que había despertado gracias al odio y el resentimiento acumulados por los habitantes subterráneos durante tanto tiempo. Cuando llegó a la superficie arrasó con el castillo, poniendo en peligro el ecosistema del escritorio y a todo ser animado. El rey, en un intento desesperado de salvar a su pueblo, dividió su cuerpo en pedazos de papel insuflados con una magia secreta capaz de derrotar a la bestia, los llamados Papiros Sagrados”. Akarigami cayó de nuevo a las profundidades y fue sellado, cayendo en el olvido hasta convertirse en un cuento para asustar a los más pequeños. Los sublevados que acompañaron a la deidad en su camino de caos y destrucción fueron desterrados a los cajones, y se construyó una enorme puerta en la entrada para evitar que volviesen a acceder al reino. 

El rey desapareció y su hijo tomó el trono, conservando los papiros en memoria a su padre. Así sucedieron las generaciones de longevos reyes hasta llegar a la actualidad, una época próspera gobernada por el príncipe Kami, hermano gemelo de Washi. Una vez más, el reino existía ajeno a las confabulaciones de los habitantes más malvados del otro lado de la Gran Puerta, que se organizaban en un culto con el objetivo de traer a Akarigami de vuelta y repetir los pasos de sus ancestros. Para lograr su cometido requerían romper los sellos de los Papiros Sagrados y un receptáculo de papel capaz de contener a la bestia. A partir de este punto comienza la trama del juego.

### Trama

Hayashi es un reino cuyos habitantes están hechos de un tipo de papel especial que les otorga carácter. El reino se mantiene a salvo gracias a unos ancestrales papiros mágicos que mantienen el orden. El equilibrio de Hayashi se ve amenazado cuando el Culto de la Llama quebranta la gran puerta e irrumpe en el pueblo para robar los papiros y secuestrar al príncipe Kami, hermano de Washi, para emplearlo como receptáculo y resucitar a Akarigami, una deidad temible que pretende destruir el mundo. 

Los jugadores acompañarán al pequeño Washi, un guerrero origami cuyo objetivo es liberar a su hermano de las garras del culto y recuperar los papiros sagrados para devolver el orden al reino y sellar a Akarigami.



## 5. Game Flow

En una partida de Project Paper se distinguen dos tipos de espacios según su función por los que el jugador irá rotando: 
- **Lobby principal:** la partida siempre comienza en el castillo, un lugar seguro para el jugador en el que podrá aprovisionarse y planificar su estrategia para la “run” por las mazmorras. El lobby cuenta con una tienda de mejoras y una sala de entrenamiento, además de una puerta que conduce a la mazmorra.  
- **Mazmorras:** si el jugador atraviesa la puerta del lobby se encontrará con un laberinto de salas que podrá saquear y con enemigos que deberá derrotar para avanzar. Tras salir de la mazmorra, este podrá usar los tesoros obtenidos en la tienda para conseguir dinero y comprar mejoras.

### Sala de entrenamiento

En el castillo se encontrará habilitada una zona de entrenamiento que servirá como tutorial para el jugador para que este entienda las mecánicas básicas del juego. Se podrá utilizar posteriormente para poner en práctica nuevas habilidades o armas adquiridas en las mazmorras o en la tienda.

### Tienda

En la tienda el jugador será capaz de comprar mejoras permanentes para su personaje (vida, resistencia, etc) con el dinero que obtenga en las incursiones a la mazmorra. Además, se podrán mejorar las armas y pasivas de sus clases.

### Mazmorras

En las mazmorras existirán diferentes niveles, y su dificultad aumentará a medida que se avanza en ellos. Cada nivel tendrá diferentes salas con enemigos normales y una sala final con un boss. Estas salas se cerrarán al entrar el jugador y no se abrirán de nuevo hasta que todos los enemigos sean derrotados.

Derrotar enemigos en una mazmorra tendrá probabilidades de dar al jugador tesoros, los cuales aumentarán de valor con los niveles, al igual que aumenta la dificultad. 

Derrotar al boss de un nivel o piso dará varias opciones al jugador: continuar al siguiente nivel o escapar de la mazmorra. Si se decide continuar, el jugador entrará al siguiente nivel. Si se decide escapar, este será capaz de sacar los tesoros obtenidos para venderlos posteriormente en la tienda del castillo y realizar las mejoras pertinentes.



## 6. Mecánicas 

### Muerte permanente

Esta mecánica proviene del género rogue-lite. Durante la “run” por la mazmorra el personaje muere permanentemente si su medidor de vida llega a cero. Cuando esto ocurre, este perderá todos los tesoros que llevase en su inventario y regresará al castillo con la mitad de dinero que tenía antes de entrar. Difiere de los rogue-like porque algunas propiedades como el arma o las mejoras del personaje se mantienen de forma permanente, incluso cuando el personaje muere.

### Controles

El jugador será un personaje de origami con un arma, un inventario y la capacidad de moverse. Las posibles acciones a realizar se contemplan en la siguiente tabla:

| **Acción**                 | **Teclado**     | **Mando**          |
| -------------------------- | --------------- | ------------------ |
| Movimiento                 | WASD            | Joystick izquierdo |
| Dirección de ataque        | Cursor          | Joystick derecho   |
| Ataque primario            | Click izquierdo | Shoulder derecho   |
| Ataque secundario          | Click derecho   | Shoulder izquierdo |
| Dash                       | Espacio         | A/Crúz             |
| Atrás (Navegación UI)      | Escape          | B/Círculo          |
| Pausa                      | Escape          | Start              |
| Inventario                 | TAB/I           | Select             |

### Armas

El jugador tendrá diferentes clases de armas entre las que escoger que podrá desbloquear tras eliminar a los jefes. Las armas contarán con un ataque primario, uno secundario y una pasiva especial que podrán mejorarse en la tienda utilizando el dinero del jugador.

#### Espada de papel

Clase de combate cuerpo a cuerpo genérica con un estilo de juego balanceado.

- **Ataque primario**
    - **Descripción:** golpea hacia el frente, dañando a los enemigos que estén en su rango.
	- **Mejora:** aumenta el daño.
- **Ataque secundario**
    - **Descripción:** golpea girando sobre sí mismo, dañando a los enemigos que estén en el área de corte.
	- **Mejora:** aumenta el daño.
- **Pasiva**
    - **Descripción:** tras dar varios golpes, el siguiente golpe realiza daño extra.
	- **Mejora:** aumenta el daño.

#### Cerilla centelleante

Clase de combate cuerpo a cuerpo que sacrifica daño por rango y quemadura.

- **Ataque primario**
    - **Descripción:** golpea hacia el frente, dañando a los enemigos que estén en su rango.    
	- **Mejora:** aumenta el daño.
- **Ataque secundario**
    - **Descripción:** crea una explosión que daña y quema un área a su alrededor.
	- **Mejora:** aumenta la duración de la quemadura.
- **Pasiva** 
    - **Descripción:** tras dar varios golpes, el siguiente quema a los enemigos.
	- **Mejora:** aumenta la duración de la quemadura.    

#### Guantelete de chinchetas

Clase de combate cuerpo a cuerpo que sacrifica rango por daño progresivo.

- **Ataque primario**
    - **Descripción:** golpea hacia el frente, dañando a los enemigos que estén en su rango.
	- **Mejora:** aumenta el daño del golpe.
- **Ataque secundario**
    - **Descripción** realiza un plunge attack con daño en área.
	- **Mejora:** aumenta el daño del golpe.
- **Pasiva**
    - **Descripción:** dañar a un enemigo le clava una chincheta, hasta un máximo de 5, aumentando el daño de los ataques por cada chincheta clavada.
	- **Mejora:** aumenta el daño extra.

#### Grapadora

Clase de combate a rango que utiliza munición y tiene un estilo de juego balanceado.

- **Ataque primario**
    - **Descripción:** dispara una grapa hacia el frente.
	- **Mejora:** aumenta el daño de la grapa.
- **Ataque secundario**
    - **Descripción:** dispara una ráfaga de grapas hacia el frente.
	- **Mejora:** aumenta el daño de la rafaga.
- **Pasiva**
    - **Descripción:** si un enemigo está cerca, le pega con la grapadora cuerpo a cuerpo en vez de dispararle.
	- **Mejora:** aumenta el daño del golpe melee.

#### Abanico

Clase de combate a rango que no utiliza munición y sacrifica daño por golpes en área.

- **Ataque primario**
    - **Descripción:** bate el abanico para lanzar una rafaga de aire hacia el frente que daña a los enemigos en área.
	- **Mejora:** aumenta el daño de la rafaga de aire.
- **Ataque secundario**
    - **Descripción:** bate el abanico para dañar y empujar a los enemigos que tiene enfrente.
	- **Mejora:** aumenta la distancia de empuje.
- **Pasiva**
    - **Descripción:** tras lanzar varias ráfagas de aire, la siguiente empuja a los enemigos con los que colisiona.
	- **Mejora:** aumenta la distancia de empuje.

### Estados alterados

Algunos enemigos y armas del personaje serán capaces de aplicar efectos especiales, entre los que se encuentran: 
- **Quemado (al aplicar fuego):** resta una pequeña porción de vida durante unos segundos.
- **Paralizado (al aplicar pegamento):** congela al personaje por unos segundos.
- **Mojado (al aplicar agua):** el papel se reblandece y se recibe más daño.
- **Manchado (al aplicar tinta):** ralentiza el movimiento del personaje y hace que falle algunos de sus ataques.    

### Objetos

A lo largo de una “run” el jugador podrá obtener diversos objetos que otorgan una mejora, pero se pierden al morir.


| Icono | Nombre                                          | Rareza     | Descripción                                                                                               |
| ----- | ----------------------------------------------- | ---------- | --------------------------------------------------------------------------------------------------------- |
|  ![Filo Oculto](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Hidden_Blade.png?raw=true)     | Filo oculto<br><br>*Hidden blade*               | Poco común | Probabilidad de 10% (+5% por objeto extra) de disparar dos filos al usar la habilidad primaria            |
|    ![Escudo Inverso](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Inverse_Shield.png?raw=true)    | Escudo inverso<br><br>*Inverse shield*          | Poco común | Lleva un escudo en la espalda para bloquear golpes (el área incrementa un 10% por objeto extra)           |
|    ![Yesca](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Kindling.png?raw=true)    | Yesca<br><br>*Kindling*                         | Poco común | Probabilidad de 15% (+5% por objeto extra) de quemar a un enemigo                                         |
|    ![Compañero orbital](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Orbital_Buddy.png?raw=true)    | Compañero orbital<br><br>*Orbital buddy*        | Raro       | Un compañero orbitará al jugador, disparando a enemigos haciendo 50% de daño base (+25% por objeto extra) |
|     ![Folio sin doblar](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Unfolded_Sheet.png?raw=true)   | Folio sin doblar<br><br>*Unfolded sheet*        | Raro       | Al morir, vuelve a la vida con el 50% de la vida TOTAL. Se consume al usarse.                             |
|    ![Diseño Aerodinámico](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Aerodynamic_Design.png?raw=true)    | Diseño aerodinámico<br><br>*Aerodynamic design* | Común      | +25% de velocidad (+15% por objeto extra)                                                                 |
|    ![Abanico de papel](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Paper_Fan.png?raw=true)    | Abanico de papel<br><br>*Paper fan*             | Poco común | Al esquivar, lanza una brisa de aire hacia delante, empujando a enemigos                                  |
|    ![Suelas de cartón](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Cardboard_Soles.png?raw=true)   | Suelas de cartón<br><br>*Cardboard soles*       | Poco común | Hacer dash a un enemigo le hace 25% de daño base (+10% por extra)                                         |
|   ![Cutter](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Box_Cutter.png?raw=true)    | Cutter<br><br>*Box cutter*                      | Común      | +10% de daño base<br>                                                                                     |
|    ![Piel de cartulina](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Card_Stock_Skin.png?raw=true)   | Piel de cartulina<br><br>*Card stock skin*      | Común      | +10% de vida base                                                                                         |
|    ![Purpurina brillante](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Sparkling_Glitter.png?raw=true)   | Purpurina brillante<br><br>*Sparkling glitter*  | Poco común | Probabilidad de 15% (+2% por objeto extra) de aturdir a un enemigo por 5 segundos                         |
|    ![Pegamento de barra](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Glue_Stick.png?raw=true)   | Pegamento de barra<br><br>*Glue stick*          | Común      | Probabilidad de 10% (+5% por objeto extra) de pegar a un enemigo al suelo                                 |
|    ![Esponja mojada](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Wet_Sponge.png?raw=true)   | Esponja mojada<br><br>*Wet sponge*              | Común      | Probabilidad 15% (+5% por objeto extra) de debilitar a un enemigo                                         |
|   ![Pluma rota](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Items/Broken_Fountain_Pen.png?raw=true)    | Pluma rota<br><br>*Broken fountain pen*         | Poco común | Probabilidad de 10% (+5% por objeto extra) de aplicar tinta a un enemigo                                  |


### Mejoras

En la tienda se podrán obtener mejoras de los atributos del personaje de índole permanente para progresar en el juego.
- **Gramaje:** aumenta la vida.
- **Rugosidad:** reduce el daño recibido.
- **Purpurina:** reduce el cooldown del dash.    

### Tesoros

Tesoros que se pueden encontrar en salas dentro de las mazmorras y que se pueden vender en la tienda. Son el principal motor de economía dentro del juego y permite el avance del jugador. Los tesoros se organizan en categorías o tiers y, a mayor categoría, más valor. 


| Icono | Tier | Nombre                       | Valor |
| ----- | ---- | ---------------------------- | ----- |
|   ![Estrella de papel](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Treasures/Paper_Star.png?raw=true)    | 1    | Estrella de papel            | 20    |
|   ![Montón de estrellas de papel](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Treasures/Paper_Star_Pile.png?raw=true)     | 2    | Montón de estrellas de papel | 40    |
|    ![Saco de estrellas de papel ](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Treasures/Paper_Star_Bag.png?raw=true)    | 3    | Saco de estrellas de papel   | 60    |


**Ejemplos de posibles drops en niveles:**
- **Nivel 1:** drops de tier 1 (100%)
- **Nivel 2:** drops de tier 1 (70%) y 2 (30%)
- **Nivel 3:** drops de tier 1 (30%), 2 (65%) y 3 (5%)

### Feedback

Para transmitir una sensación de combate al jugador, se utilizarán tecnicas como:
- Realentizar el tiempo tras un golpe.
- Cambiar el color de los enemigos a rojo.
- Screen shake.
- Mover la cámara en dirección contraria a los golpes.
- Mostrar el daño en un pequeño texto flotante.



## 7. Level Design

Cada nivel de Project Paper equivale a un piso de la mazmorra que el jugador deberá explorar y despejar para pasar al siguiente. El  piso o nivel se genera de forma procedimental. Las salas se compondrán de escenarios interiores (dentro de los cajones del escritorio), y algunas contarán con tesoros o elementos útiles con el fin de incentivar la exploración. Sólo hay tres formas de salir de un nivel:
- **Completar el nivel derrotando al jefe**: opción de regresar al lobby o continuar al siguiente piso.
- **Morir**: perder los tesoros recogidos y la mitad del dinero. El personaje regresará al lobby y el nivel se reseteará.
- **Cerrar el juego**: el progreso realizado en el interior de la mazmorra se perderá y al reabrir el juego el personaje aparecerá en el lobby.

### Generación procedimental de mazmorras

Las mazmorras de Project Paper son creadas a partir de diferentes tipos de salas:
- **Jefe:** Salas finales de la mazmorra, aparecen en el hueco más lejano con respecto a la sala inicial, esto se hace para incentivar la exploración de la mazmorra. Al derrotar al jefe podemos progresar a la siguiente zona.
- **Tesoro:** Las salas de tesoro contienen objetos que pueden venderse al completar una run, si contienen un tesoro raro (de más valor) estas tendrán sus puertas cerradas, requiriendo una llave para abrirlas.
- **Objeto:** Las salas de objeto contienen consumibles que pueden usarse en el transcurso de la run, algunos ejemplos serían llaves o curaciones.

Estas salas se unen con salas intermedias, formando una mazmorra similar a las que podemos ver en juegos como “The Binding of Isaac” o “Cult of the Lamb”.

Al avanzar de sala en sala las puertas se irán cerrando, obligando al jugador a enfrentarse a los enemigos presentes en esta.



## 8. Enemigos & Bosses

### Enemigos

Unidades inteligentes que se encontrarán repartidas por las mazmorras. Cada clase de enemigo tendrá un comportamiento único y algunos tendrán diferentes comportamientos en función de si actúan en solitario o en grupo. En función de la dificultad que supone derrotarlos se pueden clasificar en fácil, media y alta. 

#### **Ladronzuelos**

Pequeños bichos muy rápidos que actúan de manera diferente dependiendo de los enemigos que haya en la sala.  

- **Dificultad:** baja.
- **Comportamiento:**
  - En la generación de salas, si hay varios tipos de enemigos sólo puede aparecer un ladronzuelo.
  - Si solo hay ladronzuelos en la sala, se acercan al jugador a atacar.
  - Si hay otros tipos de enemigo en la sala y solo un ladronzuelo, actúa de la siguiente manera:
    - Si el jugador no tiene oro, le ataca.
    - Si el jugador tiene oro, le roba y huye para que no lo recupere.
  - Si el jugador mata al ladronzuelo, este le devuelve lo robado.

#### **Duendecillos** 

Pequeños duendes armados con lanzas que atacan al jugador desde la distancia.  

- **Dificultad:** media.
- **Comportamiento:**
  - Si el jugador está muy lejos y no llega el rango de ataque, el duendecillo se acerca a una velocidad normal.
  - Si el jugador está muy cerca, el duendecillo se echa hacia atrás a una velocidad más lenta de lo normal mientras sigue atacando. 
  - Si el jugador está en rango, el duendecillo se queda quieto y le ataca lanzando lanzas.

#### **Caballero** 

Armadura poseída de tamaño medio que arrastra una espada gigante y realiza ataques lentos pero devastadores. Cuenta con un escudo que protege la parte frontal del personaje y deja la parte trasera al descubierto.  

- **Dificultad:** alta.
- **Comportamiento:**  
  - Si el jugador no se encuentra dentro del rango de visión, el caballero no se moverá. 
  - Si se encuentra dentro del rango de visión, se acercará al jugador con el escudo desenvainado hasta estar a una distancia corta.
  - Si el jugador está dentro de su rango de ataque, envainará el escudo dejando sus puntos débiles expuestos y realizará ataques lentos pero potentes.
  - Si el jugador ataca a melé mientras tiene el escudo envainado, este hará parry y enlazará con un ataque rápido de menor daño. Si el jugador ataca a rango, el caballero simplemente se quedará detenido para frenar la munición y justo después seguirá caminando. 

### Bosses

#### **Miso Beast**

Un animal de origami con un tamaño mayor a lo normal con diferentes fases.

- **Dificultad:** baja.
- **Comportamiento:**  
  - **Fase 1**
    - El boss se encuentra en el centro de la sala conectado por unas cadenas a 4 pilares en cada esquina. El jugador debe destruir 3 de los 4 pilares para liberarlo.
    - Al entrar en la sala aparecen varios enemigos para impedirlo. Destruir un pilar hace que aparezcan más. 
    - Al boss no se le puede dañar mientras está encadenado y, si se acerca el jugador, se hará daño y será empujado para alejarlo.
  - **Fase 2**
    - Una vez se rompan las cadenas, el boss se suelta y comienza su comportamiento normal. 
    - Para atacar, el boss apunta en la dirección del jugador, se queda quieto unos segundos para indicar que va a atacar, y carga hacia él. 
    - Una vez el boss choque contra la pared, este se stuneará a sí mismo durante un tiempo para permitir al jugador golpearle.



## 9. Arte

### Estética general

La temática será realista con estilo miniaturista, compuesto de gráficos 3D e interfaces en 2D. El escenario estará ambientado en un escritorio con material de oficina y papelería. Tanto la estética como la temática se inspiran en juegos como Tearaway, Hirogami e It Takes Two.

Los personajes estarán basados en figuras de papiroflexia. El escenario se formará usando libros y otros elementos encontrados comúnmente en un escritorio, como botes para lápices, cuadernos, etc. La mazmorra se dividirá en distintos niveles, que coincidirán con los cajones de la cajonera del escritorio.

El fondo del escenario general, es decir, el fondo del lobby será una imagen borrosa de una habitación. Las texturas de todos los elementos del escenario serán realistas, ya sean las propias paredes de la mazmorra que simularán el cartón, como las texturas de los libros que montarán el escenario. 

#### Escenarios principales

- **Lobby:** Se encuentra en la habitación de una persona (como en Toy Story, pero los personajes son miniaturas de origami). El lobby es un castillo/pueblo hecho con libros y cosas cotidianas que se pueden encontrar en una habitación.
- **Dungeons:** Hechas como si fueran de cartón, también decoradas con cosas cotidianas que se pueden encontrar en una habitación.

Los distintos elementos interactuables también incluirán la estética de escritorio. Entre las armas se podrán encontrar ejemplos como una espada de papel o una pistola grapadora y, como elementos de curación, se emplearán distintos tipos de celo.

### Arte conceptual

![Armas](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Armas.jpg?raw=true)
![Columnas](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Columnas.jpg?raw=true)
![Arte del concepto general](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/ConceptGeneral.jpg?raw=true)
![Escenario detallado](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/DetallesEscenario.jpg?raw=true)
![Ejemplo enfermería](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Enfermeria.jpg?raw=true)
![Ejemplo vendedor](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Vendedor.jpg?raw=true)

### Cámara e iluminación

Uso de una cámara isométrica, en 3º persona, con una distancia focal alta y profundidad de campo para dar efecto de que los personajes son miniaturas.



## 10. Audio

### Música

La música servirá como herramienta de refuerzo para la experiencia inmersiva. Todas las melodías deben ser seamless para poder reproducirse en bucle sin interrupciones. Se incluyen algunas referencias de videojuegos ya existentes para ejemplificar el estilo y emociones que deben suscitar las melodías.

**Tema principal:** 
- **Uso:** pantalla de Menú Principal y Lobby.
- **Descripción:** melodía ambiental tranquila. Debe ayudar a enfatizar la sensación de calma de la aldea y evocar cierta sensación de tristeza o soledad. Utilizará instrumentos de viento suaves e instrumentos de cuerdas pulsadas, como el arpa o la kalimba. Debe contar con un leitmotiv o un conjunto de notas memorable y característico que se pueda emplear en otras melodías del juego. 

**Tema de exploración:** 
- **Uso:** mazmorras.
- **Descripción:** melodía entretenida y dinámica. Debe producir una sensación de tensión, y no debe ser distractiva. El loop debe ser lo suficientemente largo como para que no se vuelva repetitivo y debe ser fácilmente combinable con el tema de combate.
- **Tema de referencia:** Darkwood OST Extended de Cult of The Lamb.

**Tema de combate:** 
- **Uso:** al entrar en combate con enemigos en las mazmorras.
- **Descripción:** variación de la melodía de exploración a la que se le añaden instrumentos de percusión y efectos sonoros. 

**Tema de la tienda:** 
- **Uso:** tienda del Hub.
- **Descripción:** melodía tranquila con un toque juguetón. Debe transmitir calidez. Variación del tema principal, añadiendo sintetizadores. 
- **Tema de referencia:** Tom Nook de Animal Crossing.

### Ambiente sonoro (SFX)

Efectos de sonido que acompañan a la música refuerzan la retroalimentación del juego. A continuación se describen los sonidos a utilizar:

#### Efectos del jugador

- **Movimiento:** sonido de dos papeles rozándose suavemente al ritmo del paso del personaje  (obtenido en pixabay.com).
- **Dash:** sonido largo de dos papeles al rozarse y efecto de corriente de aire.
- **Recibir daño:** sonido de golpe contundente y efecto de voz del personaje. Al recibir daño, por un tiempo breve el resto de sonidos se escucharán más bajos. Para referencia del efecto,  consultar Hollow Knight o Cult of the Lamb.    
- **Morir:** sonido de un papel rompiéndose y efecto de voz del personaje.

#### Efectos de armas:

- **Espada:** corte limpio y silbante de tono medio.
- **Cerilla:** igual que la clase Espada, con un tono más grave y una duración mayor.
- **Garra:** igual que la clase Espada, con un tono más agudo y una duración menor.
- **Grapadora:** estallido compacto, como el de un disparo.
- **Abanico:** silbido suave, imitando corrientes de viento.

#### Efectos de estados alterados:

- **Quemado:** crujido suave y chisporroteo de madera quemándose.
- **Paralizado:** crujido fuerte al inicio, y pequeños crujidos con un zumbido durante el efecto. Al finalizar, suena otro crujido fuerte.
- **Mojado:** sonido de burbujas y agua, como si el personaje estuviese sumergiéndose bajo el agua.
- **Manchado:** leve chapoteo viscoso al hacer cualquier movimiento.  

#### Efectos de enemigos

- **Goblin:**
	- **Por defecto:** sonidos nasales y risas burlonas.
	- **Atacar:** efecto de Woosh al tirar una lanza
	- **Recibir daño/ morir:** sonidos nasales tristes.
- **Ladronzuelo**:
	- **Por defecto:** susurros y risas agudas.
	- **Atacar:** sonido de tirar o arañar tela.
	- **Recibir daño/ morir:** grito grave.
    
#### Efectos del entorno

- **Sonido de puerta:** se reproducirá al internarse en la mazmorra. Sonará como una puerta pesada arrastrándose con efectos de eco.
- **Sonido de sala cerrándose:** al internarse en una nueva sala con enemigos. Reproducirá un golpe seco o sonidos de cadenas para indicar que las salidas han quedado bloqueadas
- **Sonidos de NPCs:** los NPCs emitirán sonidos puntualmente al interactuar con ellos. Las voces serán similares a las de los cultistas de Cult of the Lamb o los aldeanos de Animal Crossing.

### Efectos de la UI

- **Al hacer clic sobre un botón**
- **Al comprar un objeto:** sonido de monedas tintineando o sonido de caja registradora.
- **Al abrir el inventario:** efecto de sonido de libro pasando una hoja de papel.



## 11. Interfaces

### Menús

#### Menú Principal

![Menú Principal](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Main%20Menu.png?raw=true)

Pantalla inicial desde la que se podrá acceder a la partida, ajustes, etc. Componentes del Menú Principal:
- **Botón Play:** al pulsarlo conduce a la pantalla de juego. Usando el botón play de ejemplo, al hacer hover sobre un botón adquiere un fondo y se invierte el color del texto (por ejemplo, si el botón es blanco al hacer hover se volverá negro con borde blanco).
- **Botón Settings:** conduce a la pantalla de configuración.
- **Botón Credits:** conduce a los créditos del juego.
- **Botón Quit:** cierra el juego.
- **Fondo:** render del personaje principal, sobre fondo plano o escenario.
- **Número de versión:** número correspondiente a la versión del juego.
- **Dev Logo:** logo del estudio.

#### Créditos

![Créditos](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Credits.png?raw=true)

Se accede desde el menú principal, y contendrá el logo del estudio, los nombres de cada miembro del equipo y su rol principal en el desarrollo del videojuego.
- **Botón Return:** regresa a la pantalla Menú Principal.
- **Dev Logo:** logo del estudio.

#### Ajustes

![Settings](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Settings.png?raw=true)

Pantalla de ajustes a la que se podrá acceder desde la pantalla Menú Principal. Contiene los siguientes elementos:
- **Slider de música:** permite controlar el valor del volumen de la música del juego en un rango de cero a cien.
- **Slider de sonidos:** permite controlar el valor del volumen de los efectos de sonido del juego y de la interfaz en un rango de cero a cien.
- **Desplegable de idiomas:** desplegable que permite alternar el idioma del juego entre inglés (por defecto) y español.
- **Botón Return:**  regresa a la pantalla Menú Principal.

### Tienda

Se podrá acceder a esta pantalla desde el Lobby, y se compondrá de dos secciones de elementos que el jugador podrá mejorar progresivamente. Las secciones están separadas por pestañas (a modo de archivador).

#### Pestaña Armas

![Armas](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/weapon%20tab.png?raw=true)

- **Texto Weapon Upgrade:** título y descripción de la función de la pantalla.
- **Selector de clases:** debajo del texto habrá una sección de botones que permitirá escoger qué clase se desea mejorar. La clase seleccionada aparecerá resaltada en colores más claros, y para las clases bloqueadas el botón estará desactivado hasta desbloquearlas.
- **Mejoras:** para una clase existirán tres apartados a mejorar: el ataque primario, secundario y la pasiva. Estos contarán con los elementos:
- **Cuadrado Icono:** representación visual del ataque/ pasiva.
- **Nombre, nivel y descripción de la propiedad.**
- **Botón Upgrade:** mejora la propiedad indicada. Al llegar al nivel máximo el botón queda bloqueado y se indica con la frase “Already Maxed”.
- **Texto Equipped class:** con el nombre de la clase actualmente equipada
- **Imagen Render:** render del personaje con el arma equipada.

#### Pestaña Personaje

![Personaje](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Char%20tab.png?raw=true)

Similar a la Pestaña Armas, contará con los siguientes elementos:
- **Texto Character Upgrade:** título y descripción de la función de la pantalla.
- **Mejoras:** mismo que para las armas. Para una característica existen los siguientes elementos:
- **Cuadrado Icono:** representación visual de la propiedad
- **Nombre, nivel y descripción de la propiedad.**
- **Botón Upgrade:** misma función que en Pestaña Armas.
- **Imagen Render:** render del personaje.  

### Juego

![Juego](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Game.png?raw=true)

Pantalla que se mostrará durante el tiempo de juego dentro de las mazmorras. Contiene los siguientes elementos:
- **Rectángulo Life:** espacio designado para representar la vida del personaje, en forma de barra o de contenedores con forma de corazón.
- **Barra de Stamina:** situada debajo de la vida, limita la cantidad de dashes que se pueden realizar. Se recarga automáticamente.
- **Cuadrado Primary Attack:** designa el ataque primario de la clase equipada. Tiene su propio cooldown que se mostrará en el icono.
- **Cuadrado Secondary Attack:** mismo caso que el anterior pero para designar el ataque secundario.
- **Cuadrado Other:** mismo caso que el anterior pero para designar la pasiva.
- **Texto con la cantidad total de monedas adquiridas.**
- **Cuadrado Inventory:** icono que designa el inventario. Al pulsar la tecla indicada se despliega la pantalla Menú Inventario.

#### Menú Pausa (in game)

![Pausa](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/pausa1.png?raw=true)
![Pausa America de los 90](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/pausa2.png?raw=true)

Desde este menú se podrá detener la partida, realizar cambios menores en los ajustes y abandonar la partida. Contiene los mismos ajustes descritos en la pantalla de ajustes anterior. 
- **Botón Return:** desactiva el modo pausa y continúa la partida.
- **Botón Lobby:** activa un segundo panel con una advertencia. Si se pulsa sobre el botón Yes, el jugador abandonará la run y regresará al Lobby.

### Pantallas de victoria y derrota

![Stage Cleared](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Stage%20Cleared.png?raw=true)

La pantalla de victoria se mostrará al salir de la sala del boss tras haberlo derrotado, y mostrará estadísticas del jugador de dicho nivel, como las monedas e ítems recogidos y el tiempo invertido en superar dicho piso. 
- **Botón Return to Castle:** devuelve al jugador al Hub.
- **Botón Continue:** empieza un nuevo piso.

![Stage Cleared](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/Game%20Over.png?raw=true)

La pantalla Game Over se mostrará siempre que el jugador muera en combate o abandone la partida a través del Menú de Pausa. En función de la causa, la sprite del personaje se mostrará de formas distintas. 
- **Mensaje Game Over:** mensaje motivacional relacionado con la historia del juego que aparecerá debajo del letrero de Game Over.
- **Sprite Broken Character:** sprite del personaje derrotado.
- **Botón Return to Castle:** devuelve al jugador al Hub.

### Diagrama de flujo

![Diagrama de flujo](https://github.com/SuperTwinStudio/Project-Web/blob/main/PicturesGDD/flujo.png?raw=true)
