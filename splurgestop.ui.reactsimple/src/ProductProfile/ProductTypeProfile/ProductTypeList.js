import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from '../../Components/Page';
import { deleteProductType } from './ProductTypeCommands';

export function ProductTypeList() {
  const [productTypes, setProductTypes] = useState(null);
  const [productTypesLoading, setProductTypesLoading] = useState(true);

  useEffect(() => {
    const loadProductTypes = async () => {
      const url = 'https://localhost:44304/api/ProductType';
      const response = await fetch(url);
      const data = await response.json();
      setProductTypes(data);
      setProductTypesLoading(false);
    };

    loadProductTypes();
  }, []);

  const removeItem = (index) => {
    let data = productTypes.filter((_, i) => i !== index);
    setProductTypes(data);
  };

  const handleDelete = async (productType) => {
    let index = productTypes.findIndex((s) => s.id === productType.id);
    removeItem(index);

    await deleteProductType({
      id: productType.id,
    });
  };

  return (
    <Page title="Product types">
      <Link
        css={css`
          text-decoration: none;
        `}
        to={`ProductType/Add`}
      >
        {' '}
        <Button className="float-right">Add Product Type</Button>
      </Link>
      <div
        css={css`
          margin: 50px auto 20px auto;
          padding: 30px 12px;
        `}
      >
        <div
          css={css`
            display: flex;
            align-items: center;
            justify-content: space-between;
          `}
        >
          <title>Product Types</title>
        </div>
        {productTypesLoading ? (
          <div
            css={css`
              font-size: 16px;
              font-style: italic;
            `}
          >
            Loading...
          </div>
        ) : (
          <Table bordered hover size="sm">
            <thead>
              <tr
                css={css`
                  background: burlywood;
                  text-align: center;
                  text-transform: uppercase;
                `}
              >
                <th>Product type name</th>
                <th>Details</th>
                <th>Remove</th>
              </tr>
            </thead>
            {productTypes.map((productType) => (
              <tbody key={productType.id}>
                <tr>
                  <Fragment key={productType.id}>
                    <td>
                      <Link
                        css={css`
                          text-decoration: none;
                        `}
                        to={`ProductTypeInfo/${productType.id}`}
                      >
                        {productType.name}
                      </Link>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button
                        variant="info"
                        href={`ProductTypeInfo/${productType.id}`}
                      >
                        Show
                      </Button>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button
                        variant="danger"
                        onClick={() => {
                          handleDelete(productType);
                        }}
                      >
                        Delete
                      </Button>
                    </td>
                  </Fragment>
                </tr>
              </tbody>
            ))}
          </Table>
        )}
      </div>
    </Page>
  );
}
