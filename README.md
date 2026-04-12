# SE Accounting System

## Overview

SE Accounting is a secure and scalable system designed to manage company data with a strong focus on privacy, automation, and fine-grained access control.

The system enables organizations to handle sensitive business information, control employee access, and automate operations involving confidential data such as company emails and credentials.

---

## Core Concepts

### Data Privacy and Security

The system ensures that all sensitive data is handled securely:

- Encryption is applied to critical data such as emails and passwords
- No direct access to raw credentials is allowed
- Secure internal handling of authentication data

---

### Permission-Based Access Control

Access is controlled using a permission-based model rather than traditional role-based systems:

- Each employee has a customized set of permissions
- Access is granted per company, not globally
- Provides high flexibility and precise control over system usage

---

### Company Access Isolation

- Each employee can only view and interact with assigned companies
- Data isolation ensures no cross-company visibility unless explicitly granted

---

### Email Automation

The system allows employees to interact with company email accounts without exposing credentials:

- Email operations are executed internally
- Employees never see the actual email or password
- All processes are fully automated and secure

---

### Daily Expenses Tracking

A built-in system for managing employee expenses:

- Employees can record daily expenses per company
- Each record includes:
  - Amount
  - Date
  - Description
  - Responsible employee
- Enables full tracking and auditing of financial activity

---

## Architecture

The system follows Clean Architecture principles to ensure maintainability and scalability.

### Key Patterns

- Clean Architecture
- Repository Pattern
- Unit of Work
- Separation of Concerns

### Layers

```text
SE.Accounting
│
├── Domain
├── Application
├── Infrastructure
├── API
└── Frontend (wpf)
