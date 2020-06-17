import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Page } from './../Components/Page';
import { addPurchaseTransaction } from './PurchaseTransactionCommands';
import { useForm } from 'react-hook-form';
import id from 'uuid/v1';
import produce from 'immer';
import { set, has } from 'lodash';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.min.css';

function enhancedReducer(state, updateArg) {
  if (updateArg.constructor === Function) {
    return { ...state, ...updateArg(state) };
  }

  if (updateArg.constructor === Object) {
    if (has(updateArg, '_path') && has(updateArg, '_value')) {
      const { _path, _value } = updateArg;

      return produce(state, (draft) => {
        set(draft, _path, _value);
      });
    } else {
      return { ...state, ...updateArg };
    }
  }
}

const initialState = {
  transactionDate: '',
  storeId: '',
  notes: '',
  lineItems: [
    {
      id: id(),
      product: '',
      price: 0.0,
      currencyCode: 'EUR',
      currencySymbol: '€',
      booking: 'Credt',
      currencySymbolPosition: 'end',
      notes: '',
    },
  ],
};

export function NewPurchaseTransaction() {
  const [transactionLineItems, setTransactionLineItems] = useState([]);
  const [state, updateState] = React.useReducer(enhancedReducer, initialState);
  const [stores, setStores] = useState(null);
  const [storesLoading, setStoresLoading] = useState(true);

  const { handleSubmit } = useForm();

  const updateForm = React.useCallback(({ target: { value, name, type } }) => {
    const updatePath = name.split('.');

    if (updatePath.length === 1) {
      const [key] = updatePath;

      updateState({
        [key]: value,
      });
    }

    if (updatePath.length === 2) {
      updateState({
        _path: updatePath,
        _value: value,
      });
    }
  }, []);

  useEffect(() => {
    const loadStores = async () => {
      const url = 'https://localhost:44304/api/Store';
      const response = await fetch(url);
      const data = await response.json();
      setStores(data);
      setStoresLoading(false);
    };

    loadStores();
  }, []);

  const add = () => {
    setTransactionLineItems([
      ...transactionLineItems,
      {
        id: id(),
        product: '',
        price: 0.0,
        currencyCode: 'EUR',
        currencySymbol: '€',
        booking: 'Credit',
        currencySymbolPosition: 'end',
        notes: '',
      },
    ]);
  };

  const update = (e, index) => {
    let items = [...transactionLineItems];
    let item = { ...transactionLineItems[index] };
    if (e.currentTarget.name === 'price') {
      var num = Number.parseFloat(e.currentTarget.value);
      item[e.currentTarget.name] = num.toFixed(2);
    } else {
      item[e.currentTarget.name] = e.currentTarget.value;
    }
    items[index] = item;

    setTransactionLineItems(items);
  };

  const remove = (index) => {
    setTransactionLineItems([
      ...transactionLineItems.slice(0, index),
      ...transactionLineItems.slice(index + 1),
    ]);
  };
  // const totalPrice = () =>
  //   transaction.lineItems.reduce((sum, item) => sum + item.amount, 0);

  const notify = (info) => {
    toast.info(info);
  };

  const onSubmit = async () => {
    let error = await addPurchaseTransaction({
      id: null,
      transactionDate: state.transactionDate,
      storeId: state.storeId,
      notes: state.notes,
      lineItems: [...transactionLineItems],
    }).then(
      () => null,
      (purchaseTransaction) => purchaseTransaction,
    );
    if (error === null) {
      console.log(error);
      notify('Purchase transaction added');
    } else {
      toast.error(
        <div>
          Purchase transaction not added!
          <br />
          {error.message}
        </div>,
      );
    }
  };

  return (
    <Page title="Add new purchase transaction">
      <div>
        <Fragment>
          <ToastContainer />
          <div>
            <form onSubmit={handleSubmit(onSubmit)}>
              <div
                css={css`
                  margin: 1em;
                `}
              >
                <input
                  type="date"
                  className="input"
                  id="transactionDate"
                  name="transactionDate"
                  onChange={updateForm}
                  value={state.transactionDate}
                ></input>
              </div>
              {storesLoading ? (
                <div
                  css={css`
                    font-size: 16px;
                    font-style: italic;
                  `}
                >
                  Loading...
                </div>
              ) : (
                <div
                  css={css`
                    margin: 1em;
                  `}
                >
                  <label for="stores">Select a store:</label>
                  <select
                    name="storeId"
                    id="storeId"
                    className="input"
                    type="text"
                    onChange={updateForm}
                  >
                    {stores.map((store) => (
                      <option value={store.id}>{store.name}</option>
                    ))}
                  </select>
                </div>
              )}
              <div
                css={css`
                  margin: 1em;
                `}
              >
                TOTAL
                {/* Total: {totalPrice().toFixed(2)}{' '}
                {transaction.lineItems[0].currencySymbol}{' '} */}
              </div>
              <div
                css={css`
                  margin: 1em;
                  width: 50em;
                `}
              >
                <lable for="notes">Notes:</lable>
                <textarea
                  id="notes"
                  name="notes"
                  className="input"
                  type="text"
                  onChange={updateForm}
                  css={css`
                    width: 50em;
                  `}
                />
              </div>
              <div>
                <Table bordered hover size="sm">
                  <thead>
                    <tr>
                      <th>Product</th>
                      <th>Price</th>
                      <th>Notes</th>
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    {transactionLineItems.map((lineItem, idx) => (
                      <tr key={lineItem.id}>
                        <td
                          css={css`
                            height: 30px;
                            width: 33%;
                          `}
                        >
                          <input
                            type="text"
                            className="input"
                            id="product"
                            name="product"
                            onChange={(e) => update(e, idx)}
                            defaultValue={state.lineItems[idx]?.product}
                          ></input>
                        </td>
                        <td
                          css={css`
                            height: 30px;
                            width: 33%;
                          `}
                        >
                          <input
                            type="text"
                            className="input"
                            id="price"
                            name="price"
                            onChange={(e) => update(e, idx)}
                            defaultValue={state.lineItems[idx]?.price}
                          ></input>
                        </td>
                        <td
                          css={css`
                            height: 30px;
                            width: 33%;
                          `}
                        >
                          <input
                            type="text"
                            className="input"
                            id="notes"
                            name="notes"
                            onChange={(e) => update(e, idx)}
                            defaultValue={state.lineItems[idx]?.notes}
                          ></input>
                        </td>
                        <td>
                          <Button variant="danger" onClick={() => remove(idx)}>
                            -
                          </Button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </Table>
                <Button
                  onClick={() => {
                    add();
                  }}
                  variant="success"
                >
                  Add Row
                </Button>
              </div>

              <input
                type="submit"
                className="btn btn-primary"
                value="Save"
                css={css`
                  margin: 1em 0 0 0;
                  width: 5.5em;
                `}
              />
            </form>
          </div>
        </Fragment>
      </div>
    </Page>
  );
}
