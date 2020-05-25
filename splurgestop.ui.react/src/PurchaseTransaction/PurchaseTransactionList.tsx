import React, { FC } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { PurchaseTransactionData } from './PurchaseTransactionData';
import { PurchaseTransaction } from './PurchaseTransaction';
import { Table } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

interface Props {
  data: PurchaseTransactionData[];
  renderItem?: (item: PurchaseTransactionData) => JSX.Element;
}

export const PurchaseTransactionList: FC<Props> = ({ data, renderItem }) => {
  return (
    <Table bordered hover size="sm">
      <thead>
        <tr
          css={css`
            background: burlywood;
            text-align: left;
          `}
        >
          <th>Purchase date</th>
          <th>Store</th>
          <th>Total spent</th>
          <th>Item count</th>
        </tr>
      </thead>
      {data.map((transaction) => (
        <tbody key={transaction.id}>
          {renderItem ? (
            renderItem(transaction)
          ) : (
            <tr
              css={css`
                text-align: right;
              `}
            >
              <PurchaseTransaction data={transaction} />
            </tr>
          )}
        </tbody>
      ))}
    </Table>
  );
};
