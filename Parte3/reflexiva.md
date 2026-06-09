# Questões sobre Máquinas de Turing e Linguagens Sensíveis ao Contexto

## Questão 1: Por que $L_4$ não pode ser reconhecida por um Autômato de Pilha? Qual propriedade da MT possibilita o seu reconhecimento?

**Resposta:** A linguagem $L_4$ é uma linguagem sensível ao contexto, portanto não pode ser reconhecida por um AP já que o mesmo utiliza uma pilha (LIFO) como memória. O AP pode empilhar para os caracteres 'a', desempilhar para os caracteres 'b', mas perderia a quantidade de caracteres da memória para a contagem dos caracteres 'c'.

A Máquina de Turing possibilita o reconhecimento dessa linguagem devido à sua fita de leitura/escrita que atua como uma memória irrestrita e ao seu cabeçote com movimento bidirecional (esquerda e direita).

---

## Questão 2: Quantos estados foram necessários para a MT de $L_4$? Descreva a estratégia de "marcação" adotada.

**Resposta:** A MT que reconhece $L_4$ possui 9 estados. A máquina atua transformando o primeiro 'a' em 'Y' para demarcar o começo da palavra, em seguida a máquina segue indo para direita da fita enquanto houver símbolos 'a' até encontrar o primeiro 'b' que será marcado com 'X' e segue indo para direita da fita enquanto houver símbolos 'b' até encontrar o primeiro 'c' que também será marcado com 'X'. Agora se a máquina no estado 4 ler o símbolo 'c' ela volta toda a fita para a esquerda até encontrar o 'Y' e repete o mesmo processo marcando o primeiro 'a', 'b' e 'c' só que dessa vez todos com 'X' (O 'Y' é só para o primeiro símbolo), caso no estado 4 a máquina leia o símbolo '_' (vazio) a máquina entende que a palavra acabou e volta toda a fita para a esquerda com transição somente para 'X' para que assim caso encontre um símbolo 'a', 'b' ou 'c' nesse retorno final a palavra será rejeitada. Caso somente símbolos 'X' sejam lidos até chegar o 'Y' (começo da palavra) ela será aceita.

---

## Questão 3: Um computador moderno é mais poderoso que uma Máquina de Turing? Justifique à luz da Tese de Church-Turing.

**Resposta:** Não. A Tese de Church-Turing postula que qualquer algoritmo ou processo lógico que possa ser efetivamente resolvido por um computador também pode ser modelado e processado por uma Máquina de Turing. Computadores modernos executam bilhões de passos por segundo de forma paralela e em arquiteturas otimizadas, enquanto a MT opera sequencialmente, mas conceitualmente e matematicamente falando a Máquina de Turing pode resolver os exatos mesmos problemas.
