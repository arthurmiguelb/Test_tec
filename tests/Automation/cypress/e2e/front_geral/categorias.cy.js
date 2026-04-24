describe('categorias', () => {
  it('acessa a tela de categorias', () => {
    cy.visit('/categorias')
    cy.url().should('eq', 'http://localhost:5173/categorias') // verifica se de fato fui direcionado para o módulo de categorias
    cy.get('.text-2xl').should('be.visible') // verifica visibilidade do titulo do modulo
    cy.get('.overflow-x-auto').should('be.visible') // verifica existencia tabela de listagem
    cy.get('.inline-flex').should('be.visible') // verifica existencia do botão de adicionar categoria
    cy.get('.w-full > .justify-between').should('be.visible') // verifica existencia do componente de paginação
  })
  it('verifica funcionalidades de paginação', () => {
    cy.visit('/categorias')
    cy.verificarPaginacao()
  })
   it('criação de categoria', () => {
    cy.visit('/categorias')
    cy.get('.inline-flex').click() // clica no botao de adicionar categoria
    cy.get('#descricao').type('Teste categoria') // digita no campo de descrição
    cy.get('#finalidade').select('ambas')
    cy.contains('option', 'Ambas').should('exist')
    cy.get('#finalidade').select('despesa') // clica no select de finalidade
    cy.contains('option', 'Despesa').should('exist') // verifica a existencia de todos os tipos de categoria no select
    cy.get('#finalidade').select('receita')
    cy.contains('option', 'Receita').should('exist')

    cy.get('button[type="submit"]').click(); // clica em salvar categoria
    cy.get('.go2072408551').should('be.visible') // verifica se exibe mensagem de sucesso

  })
})