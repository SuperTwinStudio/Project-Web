# *< Project Paper >*
Juego de acción y exploración de mazmorras donde deberás ayudar a un pequeño origami a encontrar a su hermano en un vasto mundo fantástico. 

Trabajo desarrollado para la asignatura *Juegos para Web* del grado *Diseño y desarrollo de videojuegos* de la URJC.

## Twin Studio  
- **Portfolio:**
- **Linktree**
- **Youtube:** 
- **X (Twitter):**

## Equipo de desarrollo
- Alejandro Paniagua Moreno (@BOTPanzer) -> a.paniagua.2022@alumnos.urjc.es
- Raúl Alfonso Pérez (@racurrandom) -> r.alfonso.2022@alumnos.urjc.es
- Pablo Quiñones Gonzalez (@ThatBit345) -> p.quinones.2022@alumnos.urjc.es
- Sara María Romero Bermejo (@SaraRomBer) -> sm.romero.2022@alumnos.urjc.es
-  María Marquez García (@martesytrece) -> m.marquezg.2022@alumnos.urjc.es
- Carlos Vega San Román (@CarlosVSR) -> 

# GDD
**Versión 0.5**

## Historial de versiones
Historial de versiones y cambios realizados en el documento.
- **Versión 0.1 - 27/09/25:** esquema inicial, formato y borrador de documento. Adición de los apartados del concepto del juego, el Game Flow, Enemigos y jefes, y Tesoros.
- **Versión 0.2 - 29/09/25:** desarrollo del apartado introductorio (Concepto, Género y Público Objetivo), adición del apartado Temática en la sección de Arte, expansión de la sección Mecánicas (tutorial, tienda, mazmorras y efectos de estado). Breve desarrollo de la narrativa.
- **Versión 0.3 - 1/10/25:** revisión general de la estructura y contenido del documento. Adición de la sección Monetización y planificación. Reestructuración de sección Mecánicas (Controles del jugador, Clasificación de armas, Mejoras y tipos de tesoros). Breve desarrollo de la sección Level Design y desarrollo completo de Lore y Narrativa. 
- **Versión 0.4 - 6/10/25:** desarrollo del apartado Audio y SFX. Desarrollo del apartado Interfaces. Balanceo de las clases de armas.
- **Versión 0.5 - 11/10/25:** segunda revisión y cambios menores en todo el documento. Desarrollo del apartado Generación procedimental de mazmorras. Refinado del apartado Interfaces, Diagrama de flujo y Mejoras.

## Introducción

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

Project Paper está pensado para ejecutarse en navegadores de ordenadores a través de [itch.io](http://itch.io) y en móviles, con posibilidad de ampliar a otros mercados de la plataforma PC como Steam o Epic Store.

## Monetización y planificación

### Modelo de monetización
La versión Release del juego se lanzará en [itch.io](http://itch.io) y será free-to-play, con opción de donación a través de la plataforma. En función del grado de éxito en el desarrollo y el impacto mediático en redes sociales, se plantean más otras opciones de monetización:

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
- **Beta - (fecha por determinar):** corrección de bugs importantes, cobertura casi completa de la parte artística, testeo intensivo.  
- **Release - (fecha por determinar):** corrección de bugs identificados durante la fase de testeo, pulido de mecánicas, arte y game feel.

## Game Flow

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

## Mecánicas 

### Muerte permanente
Esta mecánica proviene del género rogue-lite. Durante la “run” por la mazmorra el personaje muere permanentemente si su medidor de vida llega a cero. Cuando esto ocurre, este perderá todos los tesoros que llevase en su inventario y regresará al castillo con la mitad de dinero que tenía antes de entrar. Difiere de los rogue-like porque algunas propiedades como el arma o las mejoras del personaje se mantienen de forma permanente, incluso cuando el personaje muere.

### Controles del Jugador
El jugador será un personaje de origami con un arma, un inventario y la capacidad de moverse. Las posibles acciones a realizar se contemplan en la siguiente tabla:

| **Acción**                 | **Teclado**     | **Mando**          |
| -------------------------- | --------------- | ------------------ |
| Movimiento                 | WASD            | Joystick izquierdo |
| Dirección de ataque        | Cursor          | Joystick derecho   |
| Efectuar ataque primario   | Click Izquierdo | Shoulder derecho   |
| Efectuar ataque secundario | Click Derecho   | Shoulder izquierdo |
| Atrás (Navegación UI)      | Escape          | B/Círculo          |
| Inventario                 | TAB/I           | X/Cuadrado         |

### Armas

El jugador tendrá diferentes clases de armas entre las que escoger que podrá desbloquear tras eliminar a los jefes. Las armas contarán con un ataque primario, uno secundario y una pasiva especial que podrán mejorarse en la tienda utilizando el dinero del jugador.

#### Clase Espada
Clase de combate cuerpo a cuerpo genérica con un estilo de juego balanceado.

**Espada de papel**
- **Ataque primario:** Golpea hacia el frente, dañando a los enemigos que estén en su rango.
	- Mejora: Aumenta el daño.
- **Ataque secundario:** Golpea girando sobre sí mismo, dañando a los enemigos que estén en el área de corte.
	- Mejora: Aumenta el daño.
- **Pasiva:** Tras dar tres golpes, el cuarto realiza daño extra.
	- Mejora: Aumenta el daño.

#### Clase Lanza
Clase de combate cuerpo a cuerpo que sacrifica velocidad por potencia y algo más de rango.

**Cerilla centelleante**
- **Ataque primario:** Golpea hacia el frente, dañando a los enemigos que estén en su rango.    
	- Mejora: Aumenta el daño.
- **Ataque secundario:** Crea una explosión que quema un área a su alrededor.
	- Mejora: Aumenta la duración de la quemadura.
- **Pasiva:** Tras cierta cantidad de ataques, el siguiente suelta una brasa que quema a los enemigos en su dirección.
	- Mejora: Aumenta la duración de la quemadura.    

#### Clase Garra
Clase de combate cuerpo a cuerpo que sacrifica área de daño por velocidad de ataque.

**Guantelete de chinchetas**
- **Ataque primario:** Golpea hacia el frente, dañando a los enemigos que estén en su rango.
	- Mejora: Aumenta el daño del golpe.
- **Ataque secundario:** Permite hacer un plunge attack con daño en área.
	- Mejora: Aumenta el daño del plunge attack.
- **Pasiva:** Tras clavar cierta cantidad de chinchetas, aumenta el daño infligido al enemigo que las tenga clavadas.
	- Mejora: Aumenta el daño extra.

#### Clase Trabuco
Clase de combate a rango que utiliza munición y tiene un estilo de juego balanceado.

**Grapadora**
- **Ataque primario:** Dispara una grapa.
	- Mejora: Aumenta el daño de la grapa.
- **Ataque secundario:** Dispara una ráfaga de 3 grapas.
	- Mejora: Aumenta el daño de la rafaga.
- **Pasiva:** Al apuntar a un enemigo cerca, le pega con la grapadora en vez de dispararle.
	- Mejora: Aumenta el daño del golpe melee.

#### Clase de Viento
Clase de combate a rango que no utiliza munición y sacrifica daño por golpes en área.

**Abanico**
- **Ataque primario:** Bate el abanico para lanzar una rafaga de aire hacia el frente que daña a los enemigos en área.
	- Mejora: Aumenta el daño de la rafaga de aire.
- **Ataque secundario:** Bate el abanico para empujar a los enemigos que tiene enfrente.
	- Mejora: Aumenta la distancia de empuje.
- **Pasiva:** Pegar por la espalda a un enemigo hace daño crítico.
	- Mejora: Aumenta el porcentaje de crítico.

#### Clase Mental
Arma de rango centrada en provocar daño de forma indirecta, generando minions u obstáculos en el mapa.

**Canica**
- **Ataque primario:** Invoca a un minion que ataca al enemigo más cercano.
	- Mejora: Aumenta el daño de los minions.
- **Ataque secundario:** Lanza la canica hacia el enemigo, le daña y regresa hacia el personaje.
	- Mejora: Aumenta el daño de la canica.
- **Pasiva:** cada cierta cantidad de invocaciones aparece un minion extra.
	- Mejora: Aumenta el daño del minion extra.

### Estados alterados
Algunos enemigos y armas del personaje serán capaces de aplicar efectos especiales, entre los que se encuentran: 
- **Quemado (al aplicar fuego):** resta una pequeña porción de vida durante unos segundos.
- **Paralizado (al aplicar pegamento):** congela al personaje por unos segundos.
- **Mojado (al aplicar agua):** el papel se reblandece y se recibe más daño.
- **Manchado (al aplicar tinta):** ralentiza el movimiento del personaje y hace que falle algunos de sus ataques.    

### Mejoras
En la tienda se podrán obtener mejoras de los atributos del personaje de índole permanente para progresar en el juego.
- **Gramaje:** aumenta la vida.
- **Rugosidad:** mejora la resistencia y reduce el daño físico recibido.
- **Absorción:** aumenta la resistencia a los estados alterados, reduciendo el daño o la duración en función del tipo.
- **Color:** otorga inmunidad al elemento correspondiente
	- **Rojo:** inmunidad al fuego
	- **Azul:** inmunidad al agua
	- **Verde:** inmunidad al pegamento
	- **Morado:** inmunidad a la tinta    

### Tesoros
Tesoros que sueltan los enemigos al morir que se pueden vender en la tienda. Son el principal motor de economía dentro del juego que permitirá el avance del jugador. Los tesoros se organizan en categorías o tiers, y a mayor categoría, más valor. 
- **Tier 1:**
	- Gotitas de celulosa.
	- Trozo de chicle.
- **Tier 2:**
	- Clip doblado.
- **TIer 3:** 
	- Pedazos de origami.
    

**Ejemplos de posibles drops en niveles:**
- **Nivel 1:** drops de tier 1 (100%)
- **Nivel 2:** drops de tier 1 (70%) y 2 (30%)
- **Nivel 3:** drops de tier 1 (30%), 2 (65%) y 3 (5%)

## Level Design

Cada nivel de Project Paper equivale a un piso de la mazmorra que el jugador deberá explorar y despejar para pasar al siguiente. El  piso o nivel se genera de forma procedimental. Las salas se compondrán de escenarios interiores (dentro de los cajones del escritorio), y algunas contarán con tesoros o elementos útiles con el fin de incentivar la exploración. Sólo hay tres formas de salir de un nivel:
- **Completar** el nivel derrotando al jefe, con opción de regresar al lobby o continuar al siguiente piso.
- **Morir**, perdiendo los tesoros recogidos y la mitad del dinero. El personaje regresará al lobby y el nivel se reseteará.
- **Cerra**r el juego, el progreso realizado en el interior de la mazmorra se perderá y al reabrir el juego el personaje aparecerá en el lobby.

### Generación procedimental de mazmorras
Las mazmorras de Project Paper son creadas a partir de diferentes tipos de salas:
- **Jefe:** Salas finales de la mazmorra, aparecen en el hueco más lejano con respecto a la sala inicial, esto se hace para incentivar la exploración de la mazmorra. Al derrotar al jefe podemos progresar a la siguiente zona.
- **Tesoro:** Las salas de tesoro contienen objetos que pueden venderse al completar una run, si contienen un tesoro raro (de más valor) estas tendrán sus puertas cerradas, requiriendo una llave para abrirlas.
- **Objeto:** Las salas de objeto contienen consumibles que pueden usarse en el transcurso de la run, algunos ejemplos serían llaves o curaciones.

Estas salas se unen con salas intermedias, formando una mazmorra similar a las que podemos ver en juegos como “The Binding of Isaac” o “Cult of the Lamb”.

Al avanzar de sala en sala las puertas se irán cerrando, obligando al jugador a enfrentarse a los enemigos presentes en esta.

## Enemigos & Bosses

### Enemigos
Unidades inteligentes que se encontrarán repartidas por las mazmorras. Cada clase de enemigo tendrá un comportamiento único y algunos tendrán diferentes comportamientos en función de si actúan en solitario o en grupo. En función de la dificultad que supone derrotarlos se pueden clasificar en fácil, media y alta. 

#### **Duendecillos** 
**Dificultad:** fácil.
**Descripción:** pequeños duendes armados con lanzas que atacan al jugador desde la distancia.
**Estados de los duendecillos:**
- Si el jugador está muy lejos y no llega el rango de ataque, el duendecillo se acerca a una velocidad normal.
- Si el jugador está muy cerca, el duendecillo se echa hacia atrás a una velocidad más lenta de lo normal mientras sigue atacando. 
- Si el jugador está en rango, el duendecillo se queda quieto y le ataca lanzando lanzas.

#### **Ladronzuelos**
**Dificultad:** media.
**Descripción:** pequeños bichos muy rápidos que actúan de manera diferente dependiendo de los enemigos que haya en la sala.
**Estados de los ladronzuelos:**
- Si solo hay ladronzuelos en la sala, vienen corriendo hacia el jugador, le atacan y se retractan lentamente para repetir el ataque. Los ladronzuelos se comunican entre ellos para coordinar atacar en diferentes momentos y no todos a la vez.
- Si hay otros tipos de enemigos en la sala, se dedican a hacer de soporte a los demás acercándose al jugador para robarle un objeto, corriendo a la otra punta de la sala y dejándolo en el suelo. Robar objetos tendría cooldown y solo lo podría hacer un ladronzuelo a la vez, por lo que se tendrían que comunicar entre ellos para ver quien lo hace. Cuando robar está en cooldown, los ladronzuelos se dedican a escapar del jugador.

#### **Caballero** 
**Dificultad:** alta.
**Descripción:** Armadura poseída de tamaño medio que arrastra una espada gigante y realiza ataques lentos pero devastadores. Cuenta con un escudo que protege la parte frontal del personaje y deja la parte trasera al descubierto.
**Estados de los caballeros:** 
- Si el jugador no se encuentra dentro del rango de visión, el caballero no se moverá. 
- Si se encuentra dentro del rango de visión, se acercará al jugador con el escudo desenvainado hasta estar a una distancia corta.
- Si el jugador está dentro de su rango de ataque, envainará el escudo dejando sus puntos débiles expuestos y realizará ataques lentos pero potentes.
- Si el jugador ataca a melé mientras tiene el escudo envainado, este hará parry y enlazará con un ataque rápido de menor daño. Si el jugador ataca a rango, el caballero simplemente se quedará detenido para frenar la munición y justo después seguirá caminando. 

### Bosses

#### **Mr. Beast**
Un animal de origami con un tamaño mayor a lo normal con diferentes fases.

**Fase 1**
- El boss se encuentra en el centro de la sala conectado por unas cadenas a 4 pilares en cada esquina. El jugador debe destruir 3 de los 4 pilares para liberarlo.
- Al entrar en la sala aparecen varios enemigos para impedirlo. Destruir un pilar hace que aparezcan más. 
- Al boss no se le puede dañar mientras está encadenado y, si se acerca el jugador, se hará daño y será empujado hacia atrás.

**Fase 2**
- Una vez se rompan las cadenas, el boss se suelta y comienza su comportamiento normal. 
- Para atacar, el boss pilla la dirección del jugador, se queda quieto unos segundos indicando que va a atacar y carga hacia él. Si el jugador se mueve, el boss puede girar ligeramente pero no mucho. 
- Una vez el boss golpee algo mientras carga, ya sea el jugador o una pared, este se stunea a sí mismo durante un tiempo, momento donde el jugador le puede golpearle. Si al cargar golpea al jugador, este será dañado y ambos se stunearán por la misma duración.

## Trasfondo

### Lore
En el trasfondo se narran los eventos previos a los eventos del juego y explican el arranque de la trama. 

La historia se desarrolla en el interior de una habitación, en un escritorio genérico de estudiante que está encantado. Todos los trozos de papel fueron alcanzados por un poder sagrado y cobraron vida. Así comenzaron una existencia pacífica sobre el tablero de la mesa, formando un reino mágico gobernado por un rey. Sin embargo, no todos los origamis se encontraban a favor de vivir bajo su gobierno, por lo que descendieron a las profundidades de los cajones del escritorio y desaparecieron durante un tiempo. Sin embargo, la oscuridad corrompió sus corazones, y cuando un día hallaron un poder malvado en el cajón más profundo, no dudaron en liberarlo y enviarlo contra el reino. Era Akarigami, deidad destructora del papel, que había despertado gracias al odio y el resentimiento acumulados por los habitantes subterráneos durante tanto tiempo. Cuando llegó a la superficie arrasó con el castillo, poniendo en peligro el ecosistema del escritorio y a todo ser animado. El rey, en un intento desesperado de salvar a su pueblo, dividió su cuerpo en pedazos de papel insuflados con una magia secreta capaz de derrotar a la bestia, los llamados Papiros Sagrados”. Akarigami cayó de nuevo a las profundidades y fue sellado, cayendo en el olvido hasta convertirse en un cuento para asustar a los más pequeños. Los sublevados que acompañaron a la deidad en su camino de caos y destrucción fueron desterrados a los cajones, y se construyó una enorme puerta en la entrada para evitar que volviesen a acceder al reino. 

El rey desapareció y su hijo tomó el trono, conservando los papiros en memoria a su padre. Así sucedieron las generaciones de longevos reyes hasta llegar a la actualidad, una época próspera gobernada por el príncipe Kami, hermano gemelo de Washi. Una vez más, el reino existía ajeno a las confabulaciones de los habitantes más malvados del otro lado de la Gran Puerta, que se organizaban en un culto con el objetivo de traer a Akarigami de vuelta y repetir los pasos de sus ancestros. Para lograr su cometido requerían romper los sellos de los Papiros Sagrados y un receptáculo de papel capaz de contener a la bestia. A partir de este punto comienza la trama del juego.

### Trama
Hayashi es un reino cuyos habitantes están hechos de un tipo de papel especial que les otorga carácter. El reino se mantiene a salvo gracias a unos ancestrales papiros mágicos que mantienen el orden. El equilibrio de Hayashi se ve amenazado cuando el Culto de la Llama quebranta la gran puerta e irrumpe en el pueblo para robar los papiros y secuestrar al príncipe Kami, hermano de Washi, para emplearlo como receptáculo y resucitar a Akarigami, una deidad temible que pretende destruir el mundo. 

Los jugadores acompañarán al pequeño Washi, un guerrero origami cuyo objetivo es liberar a su hermano de las garras del culto y recuperar los papiros sagrados para devolver el orden al reino y sellar a Akarigami.

## Arte

### Estética general
La temática será realista con estilo miniaturista, compuesto de gráficos 3D e interfaces en 2D. El escenario estará ambientado en un escritorio con material de oficina y papelería. Tanto la estética como la temática se inspiran en juegos como Tearaway, Hirogami e It Takes Two.

Los personajes estarán basados en figuras de papiroflexia. El escenario se formará usando libros y otros elementos encontrados comúnmente en un escritorio, como botes para lápices, cuadernos, etc. La mazmorra se dividirá en distintos niveles, que coincidirán con los cajones de la cajonera del escritorio.

El fondo del escenario general, es decir, el fondo del lobby será una imagen borrosa de una habitación. Las texturas de todos los elementos del escenario serán realistas, ya sean las propias paredes de la mazmorra que simularán el cartón, como las texturas de los libros que montarán el escenario. 

#### Escenarios principales
- **Lobby:** Se encuentra en la habitación de una persona (como en Toy Story, pero los personajes son miniaturas de origami). El lobby es un castillo/pueblo hecho con libros y cosas cotidianas que se pueden encontrar en una habitación.
- **Dungeons:** Hechas como si fueran de cartón, también decoradas con cosas cotidianas que se pueden encontrar en una habitación.

Los distintos elementos interactuables también incluirán la estética de escritorio. Entre las armas se podrán encontrar ejemplos como una espada de papel o una pistola grapadora y, como elementos de curación, se emplearán distintos tipos de celo.

### Arte conceptual

### Cámara e iluminación
Uso de una cámara isométrica, en 3º persona, con una distancia focal alta y profundidad de campo para dar efecto de que los personajes son miniaturas.

## Audio

### Música

La música servirá como herramienta de refuerzo para la experiencia inmersiva. Todas las melodías deben ser seamless para poder reproducirse en bucle sin interrupciones. Se incluyen algunas referencias de videojuegos ya existentes para ejemplificar el estilo y emociones que deben suscitar las melodías.

**Tema principal:** 
- **Uso:** pantalla de Menú Principal y Hub.
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
- **Movimiento:** sonido de dos papeles rozándose suavemente al ritmo del paso del personaje.
- **Dash:** sonido largo de dos papeles al rozarse y efecto de corriente de aire.
- **Recibir daño:** sonido de golpe contundente y efecto de voz del personaje. Al recibir daño, por un tiempo breve el resto de sonidos se escucharán más bajos. Para referencia del efecto,  consultar Hollow Knight o Cult of the Lamb.    
- **Morir:** sonido de un papel rompiéndose y efecto de voz del personaje.

#### Efectos de armas:
- **Espada:** corte limpio y silbante de tono medio.
- **Lanza:** igual que la clase Espada, con un tono más grave y una duración mayor.
- **Garra:** igual que la clase Espada, con un tono más agudo y una duración menor.
- **Trabuco:** estallido compacto, como el de un disparo.
- **Abanico:** silbido suave, imitando corrientes de viento.
- **Mental:** efecto con sintetizador y sonidos metálicos. 

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

## Interfaces

### Menús

#### Menú Principal
Pantalla inicial desde la que se podrá acceder a la partida, ajustes, etc. Componentes del Menú Principal:
- **Botón Play:** al pulsarlo conduce a la pantalla de juego. Usando el botón play de ejemplo, al hacer hover sobre un botón adquiere un fondo y se invierte el color del texto (por ejemplo, si el botón es blanco al hacer hover se volverá negro con borde blanco).
- **Botón Settings:** conduce a la pantalla de configuración.
- **Botón Credits:** conduce a los créditos del juego.
- **Botón Quit:** cierra el juego.
- **Fondo:** render del personaje principal, sobre fondo plano o escenario.
- **Número de versión:** número correspondiente a la versión del juego.
- **Dev Logo:** logo del estudio.

#### Créditos
Se accede desde el menú principal, y contendrá el logo del estudio, los nombres de cada miembro del equipo y su rol principal en el desarrollo del videojuego.
- **Botón Return:** regresa a la pantalla Menú Principal.
- **Dev Logo:** logo del estudio.

#### Ajustes
Pantalla de ajustes a la que se podrá acceder desde la pantalla Menú Principal. Contiene los siguientes elementos:
- **Slider de música:** permite controlar el valor del volumen de la música del juego en un rango de cero a cien.
- **Slider de sonidos:** permite controlar el valor del volumen de los efectos de sonido del juego y de la interfaz en un rango de cero a cien.
- **Desplegable de idiomas:** desplegable que permite alternar el idioma del juego entre inglés (por defecto) y español.
- **Botón Return:**  regresa a la pantalla Menú Principal.

### Tienda
Se podrá acceder a esta pantalla desde el Lobby, y se compondrá de dos secciones de elementos que el jugador podrá mejorar progresivamente. Las secciones están separadas por pestañas (a modo de archivador).
#### Pestaña Armas
- **Texto Weapon Upgrade:** título y descripción de la función de la pantalla.
- **Selector de clases:** debajo del texto habrá una sección de botones que permitirá escoger qué clase se desea mejorar. La clase seleccionada aparecerá resaltada en colores más claros, y para las clases bloqueadas el botón estará desactivado hasta desbloquearlas.
- **Mejoras:** para una clase existirán tres apartados a mejorar: el ataque primario, secundario y la pasiva. Estos contarán con los elementos:
- **Cuadrado Icono:** representación visual del ataque/ pasiva.
- **Nombre, nivel y descripción de la propiedad.**
- **Botón Upgrade:** mejora la propiedad indicada. Al llegar al nivel máximo el botón queda bloqueado y se indica con la frase “Already Maxed”.
- **Texto Equipped class:** con el nombre de la clase actualmente equipada
- **Imagen Render:** render del personaje con el arma equipada.

#### Pestaña Personaje
Similar a la Pestaña Armas, contará con los siguientes elementos:
- **Texto Character Upgrade:** título y descripción de la función de la pantalla.
- **Mejoras:** mismo que para las armas. Para una característica existen los siguientes elementos:
- **Cuadrado Icono:** representación visual de la propiedad
- **Nombre, nivel y descripción de la propiedad.**
- **Botón Upgrade:** misma función que en Pestaña Armas.
- **Imagen Render:** render del personaje.  
### Juego
Pantalla que se mostrará durante el tiempo de juego dentro de las mazmorras. Contiene los siguientes elementos:
- **Rectángulo Life:** espacio designado para representar la vida del personaje, en forma de barra o de contenedores con forma de corazón.
- **Barra de Stamina:** situada debajo de la vida, limita la cantidad de dashes que se pueden realizar. Se recarga automáticamente.
- **Cuadrado Primary Attack:** designa el ataque primario de la clase equipada. Tiene su propio cooldown que se mostrará en el icono.
- **Cuadrado Secondary Attack:** mismo caso que el anterior pero para designar el ataque secundario.
- **Cuadrado Other:** mismo caso que el anterior pero para designar la pasiva.
- **Texto con la cantidad total de monedas adquiridas.**
- **Cuadrado Inventory:** icono que designa el inventario. Al pulsar la tecla indicada se despliega la pantalla Menú Inventario.

#### Menú Pausa (in game)
Desde este menú se podrá detener la partida, realizar cambios menores en los ajustes y abandonar la partida. Contiene los mismos ajustes descritos en la pantalla de ajustes anterior. 
- **Botón Return:** desactiva el modo pausa y continúa la partida.
- **Botón Lobby:** activa un segundo panel con una advertencia. Si se pulsa sobre el botón Yes, el jugador abandonará la run y regresará al Lobby.

### Pantallas de victoria y derrota
La pantalla de victoria se mostrará al salir de la sala del boss tras haberlo derrotado, y mostrará estadísticas del jugador de dicho nivel, como las monedas e ítems recogidos y el tiempo invertido en superar dicho piso. 
- **Botón Return to Castle:** devuelve al jugador al Hub.
- **Botón Continue:** empieza un nuevo piso.

La pantalla Game Over se mostrará siempre que el jugador muera en combate o abandone la partida a través del Menú de Pausa. En función de la causa, la sprite del personaje se mostrará de formas distintas. 
- **Mensaje Game Over:** mensaje motivacional relacionado con la historia del juego que aparecerá debajo del letrero de Game Over.
- **Sprite Broken Character:** sprite del personaje derrotado.
- **Botón Return to Castle:** devuelve al jugador al Hub.

### Diagrama de flujo