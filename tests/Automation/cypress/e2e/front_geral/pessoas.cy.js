describe('pessoas', () => {
  it('acessa a tela de pessoas', () => {
    cy.visit('/pessoas')
    cy.url().should('eq', 'http://localhost:5173/pessoas') // verifica se de fato fui direcionado para o módulo de pessoas
    cy.get('.text-2xl').should('be.visible') // verifica visibilidade do titulo do modulo
    cy.get('.overflow-x-auto').should('be.visible') // verifica existencia tabela de listagem
    cy.get('.inline-flex').should('be.visible') // verifica existencia do botão de adicionar pessoa
    cy.get('.w-full > .justify-between').should('be.visible') // verifica existencia do componente de paginação
  })
  it('verifica funcionalidades de paginação', () => {
    cy.visit('/pessoas')
    cy.verificarPaginacao()
  })
  
   it('criação de pessoa', () => {
    cy.visit('/pessoas')
    cy.get('.inline-flex').click() // clica no botao de adicionar pessoa
    cy.get('[name="nome"]').type('teste automação') // digita no campo de nome
    cy.get('#dataNascimento').type('1995-08-15') // preenche o campo de data
    cy.get('.justify-end > .bg-primary').click() // clica no botao salvar
    cy.get('.go2072408551').should('be.visible') // verifica se exibe mensagem de sucesso
  })
    it('atualização de pessoa', () => {
   cy.visit('/pessoas')
    cy.contains('button', '1').click()
    cy.editarPessoa('teste automação')
    cy.get('.go2072408551').should('be.visible') // verifica se exibe mensagem de sucesso
  })

   it('deleta pessoa criada', () => {
    cy.visit('/pessoas')
    cy.contains('button', '1').click()
    cy.deletarItemPaginado('teste automação') // deleta a pessoa criada anteriormente (caso rodar apenas esse it irá retornar um erro de 'teste automação não encontrado')
  })
})