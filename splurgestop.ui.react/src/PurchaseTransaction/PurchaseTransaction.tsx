import React, { FC, Fragment } from 'react';
import { Link } from 'react-router-dom';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { PurchaseTransactionData } from './PurchaseTransactionData';

interface Props {
  data: PurchaseTransactionData;
}

export const PurchaseTransaction: FC<Props> = ({ data }) => (
  <Fragment key={data.id}>
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
  </Fragment>
);
