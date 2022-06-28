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
- Menu inicial - Para cada botão do menu, chama um método que desativa todos os menus e ativa só o referente ao botão;

- Seleção de nível
  - Com PlayerPrefs, habilita ou desabilita o botão de cada nível se o nível estiver desbloqueado, verifica o número de estrelas obtidas em cada nível; Faz a troca de páginas da seleção de nível habilitando a página atual e desabilitando as demais;

- Jogo - Começa com uma contagem regressiva, ao chegar em 0, desabilita a contagem, libera a movimentação do jogador e inicia o contador de tempo do nível;
Quando o jogador colide com um cacto, reduz a barra de vida e atualiza a hud, se a barra de vida zerar ou se o jogador cair em um abismo, perde um coração e o revive no último checkpoint; Contador de tempo e progresso do nível; Botão de pause onde o jogador pode voltar ao menu inicial, reiniciar o nível ou voltar ao jogo; Se o jogador perder todos os corações, chama a tela de fim de jogo, onde o jogador pode voltar ao menu inicial, reiniciar o nível ou chamar um ads para ganhar um coração extra e reviver no último checkpoint; 
- 
- 

# Manutenção do projeto planejada:
- 
- 
- 
- 
- 

# Tempo gasto com o projeto:
- Do dia 22* até o dia 27* foram investidas em média 8 horas por dia no projeto.
- *22 - Início do projeto.
- *27 - Fim do projeto.

# Maiores dificuldades:
- 
