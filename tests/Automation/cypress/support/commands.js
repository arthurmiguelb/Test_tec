Cypress.Commands.add('deletarItemPaginado', (nome) => {

  function buscarEDeletar() {
    cy.get('body').then($body => {

      // procura linha pelo nome (primeira coluna da tabela)
      const linha = $body.find('tbody tr td:first-child')
        .filter((i, el) => el.innerText.trim() === nome)

      if (linha.length > 0) {

        // encontrou → deleta
        cy.contains('td', nome)
          .parent('tr')
          .within(() => {
            cy.get('button.delete').click()
        })

        cy.contains('button', 'Confirmar').click() //clica em confirmar
        
      } else {

        // tenta clicar em "Próximo" se não estiver desabilitado
        const proximo = $body.find('button:contains("Próximo"):not([disabled])')

        if (proximo.length > 0) {

          cy.wrap(proximo).click()

          // espera renderizar nova página
          cy.get('table tbody').should('be.visible')

          // recursão
          buscarEDeletar()

        } else {
          throw new Error(`Item "${nome}" não encontrado em nenhuma página`)
        }

      }
    })
  }

  buscarEDeletar()
})

Cypress.Commands.add('verificarPaginacao', () => {
  cy.contains('button', 'Próximo').then($btn => {

    const isDisabled = $btn.is(':disabled')

    if (!isDisabled) { // faz uma verificação se há disponibilidade de paginação
      cy.log('Tem mais de uma página')

      cy.wrap($btn).click()

      cy.contains('button', 'Anterior')
        .should('not.be.disabled')
        .click()

    } else { // caso não exista retorna apenas a mensagem
      cy.log('Existe apenas uma página')
    }

  })
})

Cypress.Commands.add('CriaUmaPessoa', (nome) => {
    cy.visit('/pessoas')
    cy.get('.inline-flex').click() // clica no botao de adicionar pessoa
    cy.get('[name="nome"]').type(nome) // digita no campo de nome
    cy.get('#dataNascimento').type('1995-08-15') // preenche o campo de data
    cy.get('.justify-end > .bg-primary').click() // clica no botao salvar
    cy.get('.go2072408551').should('be.visible') // verifica se exibe mensagem de sucesso
})

Cypress.Commands.add('editarPessoa', (nome) => {

  function buscarEEditar() {
    cy.get('body').then($body => {

      // procura linha pelo nome (primeira coluna da tabela)
      const linha = $body.find('tbody tr td:first-child')
        .filter((i, el) => el.innerText.trim() === nome)

      if (linha.length > 0) {

        // encontrou → edita
        cy.contains('td', nome)
          .parent('tr')
          .within(() => {
            cy.get('button.edit').click()
        })

        cy.contains('button', 'Salvar').click() //clica em confirmar
        
      } else {

        // tenta clicar em "Próximo" se não estiver desabilitado
        const proximo = $body.find('button:contains("Próximo"):not([disabled])')

        if (proximo.length > 0) {

          cy.wrap(proximo).click()

          // espera renderizar nova página
          cy.get('table tbody').should('be.visible')

          // recursão
          buscarEEditar()

        } else {
          throw new Error(`Item "${nome}" não encontrado em nenhuma página`)
        }

      }
    })
  }

  buscarEEditar()
})
Cypress.Commands.add('buscarItemPaginado', (nome) => {

  function buscar() {
    cy.get('body').then($body => {

      // procura pelo item na tabela (primeira coluna)
      const linha = $body.find('tbody tr td:first-child')
        .filter((i, el) => el.innerText.trim() === nome)

      if (linha.length > 0) {

        // encontrou
        cy.log(`Item "${nome}" encontrado`)
        
        cy.contains('td', nome)
          .should('be.visible')

      } else {

        // tenta ir para próxima página
        const proximo = $body.find('button:contains("Próximo"):not([disabled])')

        if (proximo.length > 0) {

          cy.wrap(proximo).click()

          // espera tabela atualizar
          cy.get('table tbody').should('be.visible')

          // recursão
          buscar()

        } else {

          // não encontrou em nenhuma página
          throw new Error(`Item "${nome}" não encontrado em nenhuma página`)
        }
      }
    })
  }

  buscar()
})

