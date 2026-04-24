# Bug: Categoria aceita finalidade iválida

## Descrição
Apenas na API é possivel realizar a criação de uma categoria com sua finalidade portando qualquer tipo de valor seja ele **positivo** ou **negativo**


## Passos para reproduzir
1. Realizar uma requisição POST para criação de uma categoria
2. POST /api/v1.0/categorias  
{  
  "descricao": "TESTE EVIDÊNCIA",  
  "finalidade": -10000  
}


## Resultado atual
-  Categoria criada com sucesso
- Front exibe categoria criada com finalidade `Ambas`

## Resultado esperado
- Validar enum `(0,1,2)`
- Retornar erro para valores inválidos
## Evidências
![alt text](images/image.png)
![alt text](images/image-1.png)
![alt text](images/image-2.png)

## Ambiente
- API: http://localhost:5000
- Front: http://localhost:5173
- Navegador: Chrome
- Versão: v1
