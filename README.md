5. Explicar o que acontece com a lista de filmes se ele derrubar o servidor e subir novamente.
Resposta: Os dados não são persistidos. Ao reiniciar o servidor, a lista de filmes é apagada, pois o banco InMemory armazena tudo apenas na memória temporária da aplicação.
