import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../Components/Page';
import { addPurchaseTransaction } from './PurchaseTransactionCommands';
import { formatDate } from './../Common/DateTimeHelpers';

export function NewPurchaseTransaction() {
  const [transaction, setTransaction] = useState(null);

  const handleInputChange = (event) => {
    setTransaction({
      id: null,
      purchaseDate: null,
      store: null,
      lineItems: null,
    });
  };

  const handleSubmit = async () => {
    await addPurchaseTransaction({
      id: null,
      name: transaction.name,
    });
  };

  return (
    <Page title="Add new purchase transaction">
      <div>
        <Fragment>
          <div>
            <form>
              <p>
                <b>
                  <input
                    type="date"
                    id="purchaseDate"
                    name="purchaseDate"
                  ></input>
                </b>
                &nbsp;&nbsp;&nbsp;
                <div>
                  <input type="text" name="store.name"></input>
                </div>
              </p>
              <div>Total: Sum total price here </div>
              <div>
                <textarea name="notes" />
              </div>
              <div>
                <Table bordered hover size="sm">
                  <thead>
                    <tr>
                      <th>Product</th>
                      <th>Price</th>
                      <th>Notes</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td>Product name goes here...</td>
                      <td>price here</td>
                      <td>notes</td>
                    </tr>
                    <tr>+</tr>
                  </tbody>
                </Table>
              </div>
              <button type="submit">Save</button>
            </form>
          </div>
        </Fragment>
      </div>
    </Page>
  );
}
