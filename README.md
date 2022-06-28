<h1 align="center">Endless Runner - Teste Liga</h1>
<h1 align="center">Documentação</h1>

# Principais Classes:
- Menus - Hud do menu e suas transições;
- LevelSelector - Verificação dos níveis se desbloqueados ou não e número de estrelas;
- Player - Junto com as classes PlayerMovementMobile, são responsáveis pela movimentação do jogador e animações;
- Health - Vida e hud de vida do jogador;
- LevelMananger - Toda a jogabilidade do nível, checkpoints e hud;
- CheckPoint - Animação da bandeira de checkpoint e por passar seu transform para a classe LevelManager;
- Finish - Animação da bandeira do final e por encerrar o nível chamando o método LevelFinished() da classe LevelManager;
- DataManager - Salvar os níveis desbloqueados, completos e suas estrelas;
- AdsInitializer - Iniciar o Ads;
- RewardedAdsButton - Chama o Ads de reward e se completo, chama um evento que revive o jogador com uma vida bônus no último checkpoint;
- AnalyticsSender - Mandar um analytics sempre quando iniciar um nível e quando o nível for completo.

# Lógicas Empregadas:
- Menu inicial
  - Para cada botão do menu, chama um método que desativa todos os menus e ativa só o referente ao botão;

- Seleção de nível
  - Com PlayerPrefs, habilita ou desabilita o botão de cada nível se o nível estiver desbloqueado, verifica o número de estrelas obtidas em cada nível (If pegando as keys do PlayerPrefs);
  - Faz a troca de páginas da seleção de nível habilitando a página atual e desabilitando as demais;
  - Botão para cada nível (LoadScene para o nível escolhido);

- Jogo
  - Começa com uma contagem regressiva com (-= Time.deltaTime), ao chegar em 0, desabilita a contagem, libera a movimentação do jogador (Deixando uma variável bool da classe Player verdadeira) e inicia o contador de tempo do nível com;
  - Quando o jogador colide com um cacto, reduz a barra de vida e atualiza a hud, se a barra de vida zerar ou se o jogador cair em um abismo, perde um coração e o revive no último checkpoint (Transform do player igual ao transform do checkpoint);
  - Contador de tempo (+= Time.deltaTime) e progresso do nível (distância entre o player );
  - Botão de pause (Time.timeScale = 0) onde o jogador pode voltar ao menu inicial (LoadScene para o menu inicial), reiniciar o nível (LoadScene no mesmo nível) ou voltar ao jogo (Desabilita a hud e Time.timeScale = 1); 

- Fim de jogo
  - Se o jogador perder todos os corações, chama a tela de fim de jogo, onde o jogador pode voltar ao menu inicial (LoadScene para o menu inicial), reiniciar o nível (LoadScene no mesmo nível) ou chamar um ads para ganhar um coração extra e reviver no último checkpoint (Transform do player igual ao transform do checkpoint);

- Nível Completo
  - Mostra o tempo levado para chegar ao final do nível e o número de corações restantes.
  - Faz uma verificação dos dados a cima (If), recompensa o jogador com estrelas de acordo com seu tempo e corações restantes e salva alguns dadosno PlayerPrefs, (Nível completo, número de níveis desbloquados++, e número de estrelas).
  - O jogador pode escolher voltar ao menu inicial (LoadScene para o menu inicial), reiniciar o nível (LoadScene no mesmo nível), ou ir ao próximo nível (LoadScene no próximo nível).

# Manutenção do projeto planejada:
- Novos Desafios
  - Novos níveis;
  - Novos obstáculos;
- Melhorias de Qualidade de Vida
  - Melhoria na movimentação do jogador;
  - Melhoria nas animações do jogador;
  - Melhoria na hud;
  - Animações na hud;
  - Melhorar o Analytics;
  - Correção de bugs.

# Tempo gasto com o projeto:
- 22 - Início do projeto;
- 27 - Fim do projeto;

- Jogador
  - Programação - 4 horas;
  - Animação - 1 hora;
- Hud
  - Menu Inicial - 1 hora;
- Testes - 8 horas;

# Maiores dificuldades:
- Documentação.
