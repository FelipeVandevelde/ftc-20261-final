# Questões sobre Autômatos com Pilha e Linguagens Livres de Contexto

## Questão 1: Porque $L_2$ não pode ser reconhecida por um AFD? Demonstre usando o Lema do Bombeamento para linguagens regulares.

**Resposta:** Suponha que $L_2$ seja regular. Pelo Lema do Bombeamento, existe um número p tal que qualquer palavra de comprimento maior ou igual a p pode ser dividida em $xyz$, obedecendo:

- $|xy| ≤ p$
- $|y| > 0$
- $xy^iz ∈ L_2$ para todo $i ≥ 0$

Escolhendo a palavra $w = a^p b^p$, a parte $y$ estará necessariamente dentro da sequência de símbolos 'a', pois $|xy| ≤ p$.

Ao bombear $i = 2$, obtemos: $a^(p+|y|) b^p$.

Agora a quantidade de 'a' é diferente da quantidade de 'b', logo a palavra não pertence mais a $L_2$. Isso contradiz o Lema do Bombeamento. Portanto, $L_2$ não é regular e não pode ser reconhecida por um AFD.

---

## Questão 2: A aceitação por pilha vazia e por estado final são equivalentes em poder de reconhecimento? Demonstre conceitualmente.

**Resposta:** Sim. Os dois critérios possuem o mesmo poder de reconhecimento para linguagens livres de contexto. Na aceitação por estado final, a palavra é aceita quando o processamento termina em um estado marcado como final. Na aceitação por pilha vazia, a palavra é aceita quando toda a entrada é consumida e a pilha fica vazia.

Qualquer AP que aceita por estado final pode ser transformado em outro AP equivalente que aceita por pilha vazia, e vice-versa. Para isso, basta adicionar estados e transições auxiliares que convertem um critério no outro sem alterar a linguagem reconhecida. Assim, ambos reconhecem exatamente a mesma classe de linguagens.

---

## Questão 3: Explique o papel do símbolo $Z_0$ na sua implementação.

**Resposta:** O símbolo $Z_0$ representa o marcador inicial da pilha. Ele é colocado na pilha antes do início do processamento e serve para indicar o fundo da pilha. Na implementação, o símbolo 'Z' permite distinguir uma pilha realmente vazia de uma pilha que ainda contém elementos empilhados durante o processamento.

Além disso, ele é utilizado nas transições para identificar quando todos os símbolos relevantes já foram removidos da pilha. Ao final do processamento, uma λ-transição remove o $Z_0$, deixando a pilha completamente vazia e permitindo a aceitação por pilha vazia.