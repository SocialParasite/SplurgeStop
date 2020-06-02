import React, { FC, useState, Fragment, useEffect, ChangeEvent } from 'react';
import { Page } from './../Page';
import { RouteComponentProps } from 'react-router-dom';
import {
  DetailedPurchaseTransactionData,
  getPurchaseTransaction,
} from './PurchaseTransactionData';
import { Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

interface RouteParams {
  id: string;
}

export const PurchaseTransactionPage: FC<RouteComponentProps<RouteParams>> = ({
  match,
}) => {
  const [
    purchaseTransaction,
    setPurchaseTransaction,
  ] = useState<DetailedPurchaseTransactionData | null>(null);

  const [isEditing, setEditing] = useState(false);

  const [selectedDate, setSelectedDate] = useState(
    new Date().toLocaleDateString().split('/').reverse().join('-'),
  );

  useEffect(() => {
    const doGetPurchaseTransaction = async (id: string) => {
      const foundPurchaseTransaction = await getPurchaseTransaction(id);
      setPurchaseTransaction(foundPurchaseTransaction);
    };

    if (match.params.id) {
      const purchaseTransactionId = match.params.id;
      doGetPurchaseTransaction(purchaseTransactionId);
    }
  }, [match.params.id]);

  const editModeClick = () => {
    setEditing(!isEditing);
  };

  const currentDate = (e: React.ChangeEvent<HTMLInputElement>) => {
    var newDate = new Date(String(e.target.value))
      .toLocaleDateString()
      .split('/')
      .reverse()
      .join('-');
    setSelectedDate(newDate);
  };

  const changeHandler = (e: React.FormEvent<HTMLInputElement>) => {
    // setPurchaseTransaction({purchaseTransaction.store["name"]: e.currentTarget.value });
    // [e.currentTarget.name]: e.currentTarget.value
    console.log(e.currentTarget.name);
    console.log(e.currentTarget.value);
  };

  return (
    <Page>
      <button onClick={editModeClick}>Edit</button>
      <div>
        {purchaseTransaction !== null && (
          <Fragment>
            <div>
              {isEditing ? (
                <form>
                  <p>
                    <b>
                      <input
                        type="date"
                        id="purchaseDate"
                        value={selectedDate}
                        name="purchaseDate"
                        onChange={currentDate}
                      ></input>
                    </b>
                    <i>
                      {new Date(
                        String(
                          purchaseTransaction.purchaseDate.value.slice(0, 10),
                        ),
                      )
                        .toLocaleDateString()
                        .split('/')
                        .join('.')}
                    </i>
                    )} &nbsp;&nbsp;&nbsp;
                    <div>
                      {/* TODO: Change to drop-down. Load values after edit mode has been activated */}
                      <input
                        type="text"
                        name="store.name"
                        value={purchaseTransaction.store.name}
                        onChange={changeHandler}
                      ></input>
                    </div>
                  </p>
                  <div>Total: {purchaseTransaction.totalPrice} </div>
                  <div>
                    <textarea name="notes" value={purchaseTransaction.notes} />
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
                        {purchaseTransaction.lineItems.map((item) => {
                          return (
                            <tr key={item.id}>
                              <td>Product name goes here...</td>
                              <td>
                                {item.price.currency.positionRelativeToPrice ===
                                'end'
                                  ? String(
                                      item.price.amount +
                                        ' ' +
                                        item.price.currency.currencySymbol,
                                    )
                                  : String(
                                      item.price.currency.currencySymbol +
                                        ' ' +
                                        item.price.amount,
                                    )}
                              </td>
                              <td>{item.price.currency.currencyCode}</td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </Table>
                  </div>
                  <button type="submit">Save</button>
                </form>
              ) : (
                <div>
                  <p>
                    <b>
                      <input
                        type="date"
                        id="purchaseDate"
                        value={selectedDate}
                        name="purchaseDate"
                        onChange={currentDate}
                      ></input>
                    </b>
                    <i>
                      {new Date(
                        String(
                          purchaseTransaction.purchaseDate.value.slice(0, 10),
                        ),
                      )
                        .toLocaleDateString()
                        .split('/')
                        .join('.')}
                    </i>
                    )} &nbsp;&nbsp;&nbsp;
                    <i>{purchaseTransaction.store.name}</i>
                  </p>
                  <div>Total: {purchaseTransaction.totalPrice} </div>
                  <div>
                    <p>{purchaseTransaction.notes}</p>
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
                        {purchaseTransaction.lineItems.map((item) => {
                          return (
                            <tr key={item.id}>
                              <td>Product name goes here...</td>
                              <td>
                                {item.price.currency.positionRelativeToPrice ===
                                'end'
                                  ? String(
                                      item.price.amount +
                                        ' ' +
                                        item.price.currency.currencySymbol,
                                    )
                                  : String(
                                      item.price.currency.currencySymbol +
                                        ' ' +
                                        item.price.amount,
                                    )}
                              </td>
                              <td>{item.price.currency.currencyCode}</td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </Table>
                  </div>
                </div>
              )}
            </div>
          </Fragment>
        )}
      </div>
    </Page>
  );
};
