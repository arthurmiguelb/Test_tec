describe('categorias', () => {

  it('Cria categoria, valida via API (GET por ID) e valida no front', () => {

    let idCriado
    const nomeCategoria = `Categoria_${Date.now()}`

    // Realzia a criação da categoria via API
    cy.request({
      method: 'POST',
      url: `${Cypress.env('apiUrl')}/api/v1.0/categorias`,
      body: {
        descricao: nomeCategoria,
        finalidade: 0
      }
    }).then((response) => {

      expect(response.status).to.eq(201)
      idCriado = response.body.id // guarda o ID criado para validação futura

      // valida direto na api
      cy.request({
        method: 'GET',
        url: `${Cypress.env('apiUrl')}/api/v1.0/categorias/${idCriado}`
      }).then((res) => {

        expect(res.status).to.eq(200) 
        expect(res.body.id).to.eq(idCriado) // confirma que o ID retornado é o mesmo do criado
        expect(res.body.descricao).to.eq(nomeCategoria)
        expect(res.body.finalidade).to.eq(0)
      })

      // faz validacao no front
      cy.visit('/categorias')
      cy.contains('button', '1').click()
      cy.buscarItemPaginado(nomeCategoria) 
    })
  })
})


