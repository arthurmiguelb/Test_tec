describe('transacoes', () => {
  it('acessa a tela de transações', () => {
    cy.visit('/transacoes')
    cy.url().should('eq', 'http://localhost:5173/transacoes')
    cy.get('.text-2xl').should('be.visible')
  })
  
  it('verifica funcionalidades de paginação', () => {
    cy.visit('/transacoes')
    cy.verificarPaginacao()
  })
  
  it('criação de transação de despesa', () => {
    cy.visit('/transacoes')
    cy.get('.inline-flex').click()
    cy.get('#descricao').type('Teste Receita')
    cy.get('#valor').type('500.00')
    cy.get('[name="data"]').type('2024-06-01')
    cy.get('#tipo').select('despesa')
    cy.get(':nth-child(5) > .relative > .flex > .px-3').click() 
    cy.get('#pessoa-select').type('Maria Santos')
    cy.get('#pessoa-select-options').contains('Maria Santos').click()
    cy.get('#categoria-select').type('Alimentação')
    cy.get('#categoria-select-options').contains('Alimentação').click()
    
    cy.get('button[type="submit"]').click() // após salvar irá retornar erro 500
    cy.get('.go2072408551').should('be.visible')
  })
  
  it('criação de transação de receita', () => {
    cy.visit('/transacoes')
    cy.get('.inline-flex').click()
    cy.get('#descricao').type('Teste Receita')
    cy.get('#valor').type('500.00')
    cy.get('[name="data"]').type('2024-06-01')
    cy.get('#tipo').select('receita')
    cy.get(':nth-child(5) > .relative > .flex > .px-3').click() 
    cy.get('#pessoa-select').type('Maria Santos')
    cy.get('#pessoa-select-options').contains('Maria Santos').click()
    cy.get('#categoria-select').type('Alimentação')
    cy.get('#categoria-select-options').contains('Alimentação').click()
  
    cy.get('button[type="submit"]').click() // após salvar irá retornar erro 500
    cy.get('.go2072408551').should('be.visible')
  })
  
  it('filtro de transações por tipo', () => {
    cy.visit('/transacoes')
    cy.get('#filtroTipo').select('despesa')
    cy.get('table tbody tr').should('have.length.greaterThan', 0)
  })
})
