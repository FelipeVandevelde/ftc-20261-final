# Questões sobre Autômatos Finitos Determinísticos

## Questão 1: Por que a linguagem $L_1$ é regular? Apresente a expressão regular equivalente.

**Resposta:** A linguagem $L_1$ é regular, pois, de acordo com o Teorema de Kleene, uma linguagem é classificada como regular se, e somente se, puder ser reconhecida por um autômato finito é descrita por uma expressão regular (ER). Nesse sentido, o Autômato Finito Determinístico (AFD) correspondente foi implementado no código com quatro estados (três válidos e um estado morto). Adicionalmente, a expressão regular que descreve essa linguagem é definida como: `(a+b)*ab`.

---

## Questão 2: Descreva formalmente a 5-tupla do AFD construído, justificando a escolha de cada estado.

**Resposta:** A 5-tupla do Autômato Finito Determinístico (AFD) construído é definida pelo conjunto $M=(Q, \Sigma, \delta, q_0, F)$, onde:

- **$Q=\{q_{-1}, q_0, q_1, q_2\}$** - Conjunto de estados. O estado $q_{-1}$ é um estado morto; quando o autômato o atinge, significa que a palavra não é aceita. Os outros três estados $q_0$, $q_1$, $q_2$ são os estados mínimos necessários para o funcionamento do AFD.

- **$\Sigma = \{ a, b \}$** - Alfabeto definido para a linguagem $L_1$.

- **$\delta$** - Função de transição. As transições dos estados $q_0$, $q_1$, $q_2$ para os caracteres válidos do alfabeto estão definidas de acordo com a tabela:
  - $\delta(q_0, a) = q_1$
  - $\delta(q_1, a) = q_1$
  - $\delta(q_2, a) = q_1$
  - $\delta(q_0, b) = q_0$
  - $\delta(q_1, b) = q_2$
  - $\delta(q_2, b) = q_0$

  **Transição para caracteres inválidos:** A notação $\delta(*, *) = q_{-1}$ demonstra que, se qualquer estado ler um caractere que não existe no alfabeto $\forall q \in Q, \forall e \in \Sigma \Rightarrow \delta(q, e) = q' \neq \emptyset$, o autômato transita para q₋₁. Como este é um estado morto e não é um estado de aceitação, isso indica que a palavra possui caracteres inválidos.

- **$q_0$** - Estado Inicial. O estado inicial do autômato é $q_0$.

- **$F=\{q_2\}$** - Conjunto de estados de aceitação. O único estado de aceitação é $q_2$.

---

## Questão 3: O que aconteceria se a função $\delta$ não fosse total? Como você tratou entradas inválidas (símbolos fora do alfabeto)?

**Resposta:** Se a função de transição não fosse total, o autômato poderia travar ou rejeitar palavras que deveriam ser aceitas. Além disso, ele deixaria de ser um autômato determinístico, pois haveria inconsistências em seu comportamento. Para tratar as entradas inválidas, criei uma transição que atende à condição lógica $\forall q \in Q, \forall e \in \Sigma \Rightarrow \delta(q, e) = q' \neq \emptyset$. Essa regra estabelece que, a partir de qualquer estado, a leitura de um caractere não pertencente ao alfabeto direciona o processamento para $q_{-1}$, que atua como um estado morto e encerra a execução do autômato.
