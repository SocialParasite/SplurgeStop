import React, { Fragment, useEffect, useState } from 'react';
/** @jsx jsx */
import { css, jsx } from '@emotion/core';
import { Table, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Link } from 'react-router-dom';
import { Page } from './../../Components/Page';
import { deleteBrand } from './BrandCommands';

export function BrandList() {
  const [brands, setBrands] = useState(null);
  const [brandsLoading, setBrandsLoading] = useState(true);

  useEffect(() => {
    const loadBrands = async () => {
      const url = 'https://localhost:44304/api/Brand';
      const response = await fetch(url);
      const data = await response.json();
      setBrands(data);
      setBrandsLoading(false);
    };

    loadBrands();
  }, []);

  const removeItem = (index) => {
    let data = brands.filter((_, i) => i !== index);
    setBrands(data);
  };

  const handleDelete = async (brand) => {
    let index = brands.findIndex((s) => s.id === brand.id);
    removeItem(index);

    await deleteBrand({
      id: brand.id,
    });
  };

  return (
    <Page title="Brands">
      <Link
        css={css`
          text-decoration: none;
        `}
        to={`Brand/Add`}
      >
        {' '}
        <Button className="float-right">Add Brand</Button>
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
          <title>Brands</title>
        </div>
        {brandsLoading ? (
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
                <th>Brand name</th>
                <th>Details</th>
                <th>Remove</th>
              </tr>
            </thead>
            {brands.map((brand) => (
              <tbody key={brand.id}>
                <tr>
                  <Fragment key={brand.id}>
                    <td>
                      <Link
                        css={css`
                          text-decoration: none;
                        `}
                        to={`BrandInfo/${brand.id}`}
                      >
                        {brand.name}
                      </Link>
                    </td>
                    <td
                      css={css`
                        width: 5em;
                      `}
                    >
                      <Button variant="info" href={`BrandInfo/${brand.id}`}>
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
                          handleDelete(brand);
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
