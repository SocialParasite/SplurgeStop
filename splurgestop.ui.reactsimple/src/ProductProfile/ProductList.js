import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from './../Components/Page';
import { deleteProduct } from './ProductCommands';

export function ProductList() {
  const [products, setProducts] = useState(null);
  const [productsLoading, setProductsLoading] = useState(true);

  useEffect(() => {
    const loadProducts = async () => {
      const url = 'https://localhost:44304/api/Product';
      const response = await fetch(url);
      const data = await response.json();
      setProducts(data);
      setProductsLoading(false);
    };

    loadProducts();
  }, []);

  const removeItem = (index) => {
    let data = products.filter((_, i) => i !== index);
    setProducts(data);
  };

  const handleDelete = async (product) => {
    let index = product.findIndex((s) => s.id === product.id);
    removeItem(index);

    await deleteProduct({
      id: product.id,
    });
  };

  return (
    <Page title="Products">
      <Link
        css={css`
          text-decoration: none;
        `}
        to={`Product/Add`}
      >
        {' '}
        <Button className="float-right">Add Product</Button>
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
          <title>Products</title>
        </div>
        {productsLoading ? (
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
                <th>Product name</th>
                <th>Details</th>
                <th>Remove</th>
              </tr>
            </thead>
            {products.map((product) => (
              <tbody key={product.id}>
                <tr>
                  <Fragment key={product.id}>
                    <td>
                      <Link
                        css={css`
                          text-decoration: none;
                        `}
                        to={`ProductInfo/${product.id}`}
                      >
                        {product.name}
                      </Link>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button variant="info" href={`ProductInfo/${product.id}`}>
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
                          handleDelete(product);
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
