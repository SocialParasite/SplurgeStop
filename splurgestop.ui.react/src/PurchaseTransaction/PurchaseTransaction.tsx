import React, { FC } from 'react';
import { Link } from 'react-router-dom';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { PurchaseTransactionData } from './PurchaseTransactionData';

interface Props {
  data: PurchaseTransactionData;
}

export const PurchaseTransaction: FC<Props> = ({ data }) => (
  <tr
    css={css`
      text-align: right;
    `}
  >
    <td>
      <Link
        css={css`
          text-decoration: none;
        `}
        to={`PurchaseTransaction/${data.id}`}
      >
        {data.purchaseDate.toLocaleDateString()}
      </Link>
    </td>
    <td>{data.storeName}</td>
    <td>{data.totalPrice}</td>
    <td>{data.itemCount}</td>
  </tr>
);
