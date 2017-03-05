tree grammar F2003TreeParserGenerated;

options
{
  ASTLabelType =pANTLR3_BASE_TREE;
  language = C;
  tokenVocab = F2003ParserGenerated;
}

@treeparser::includes
{
#include "Reprise/Reprise.h"
#include "GrammarObjects.h"
#include "HirFTypes.h"
#include "HirFExpressions.h"

#include <memory>

using std::string;
using std::list;
using namespace OPS::Fortran2003Parser;
using namespace OPS::Reprise;
using namespace OPS::Fortran2003Parser::Canto;
using std::auto_ptr;

}

@treeparser::members
{
#include "FParser.h"

namespace OPS
{
namespace Fortran2003Parser
{	
  struct FortranTreeParserBase;
  class FortranParser;
}
}

using OPS::Fortran2003Parser::FortranTreeParserBase;
using OPS::Fortran2003Parser::FortranParser;

#define TREE_PARSER_BASE	((FortranTreeParserBase*)ctx->pTreeParser->super) 
#define ACTIONS 		(TREE_PARSER_BASE->getActions())
#define PROCESSOR   		(TREE_PARSER_BASE->processor)

#define NEAREST_DECLARATIONS (TREE_PARSER_BASE->nearestDeclarations)
#define getTokenText(token) string((char*)token->getText(token)->chars)
}


// $<Section 2

// R201
program
returns [TranslationUnit *res]
@init 
{ 
  res = new TranslationUnit;
  res->setSourceFilename(PROCESSOR->getSourceFilePath());
}
  :  (program_unit { ACTIONS->program(res, $program_unit.res); })* 		
  ;

// R202
program_unit
returns [DeclarationBase *res]
  :  main_program { res = $main_program.res; }
//|  external_subprogram
//|  module
//|  block_data
  ;

// R204
specification_part
returns [Declarations *res]
@init { res = NEAREST_DECLARATIONS; }
  :	//( use_stmt {numUseStmts++;})*
		//( import_stmt {numImportStmts++;})*
     ^(T_SPECIFICATION_PART ( declaration_construct 
         { ACTIONS->specification_part(res, $declaration_construct.res); } )* )
  ;

// R207
declaration_construct
returns [Declarations *res]
  :
//	:	entry_stmt
//	|	parameter_stmt
//	|	format_stmt
//	|	implicit_stmt  
        // implicit_stmt must precede all occurences of the below
//	|	derived_type_def
//	|	enum_def
//	|	interface_block
//	|	procedure_declaration_stmt
//	|	specification_stmt
     derived_type_def { res = $derived_type_def.res; }
  |  type_declaration_stmt { res = $type_declaration_stmt.res; }
//	|	stmt_function_stmt
  ;

// R208
execution_part
returns [BlockStatement *res]
@init { res = new BlockStatement; }
  :  ^(T_EXECUTION_PART (execution_part_construct
        { ACTIONS->execution_part(res, $execution_part_construct.res); } )* )
  ;

// R209
execution_part_construct
returns [StatementBase* res]
@init { string *label_ = 0; }
@after { delete label_; }
  :  (label { label_ = $label.res; })? executable_construct  
     { 
        res = $executable_construct.res; 
	if (label_) {
	   res->setLabel(*label_);   
	   TREE_PARSER_BASE->addLabeledStatement(res);
	}
	
     }
//	|	format_stmt
//	|	entry_stmt
//	|	data_stmt
  ;
	
// R213
executable_construct
returns [StatementBase* res]
@after 
{
  if (!res)
    res = new EmptyStatement;
}
  :  action_stmt   { res = $action_stmt.res; }
  |  do_construct  { res = $do_construct.res; }
  |  if_construct  { res = $if_construct.res; }
//	|	associate_construct
//	|	case_construct

//	|	forall_construct

//	|	select_type_construct
//	|	where_construct
  ;
	
// R214
action_stmt
returns [StatementBase* res]
  ://	allocate_stmt
     assignment_stmt 	{ res = $assignment_stmt.res; }
  |  cycle_stmt { res = $cycle_stmt.res; }
  |  exit_stmt  { res = $exit_stmt.res; }
/*	|	backspace_stmt
	|	call_stmt
	|	close_stmt
	|	continue_stmt
	|	cycle_stmt
	|	deallocate_stmt
	|	endfile_stmt
	|	exit_stmt
	|	flush_stmt
	|	forall_stmt
	|	goto_stmt
	|	if_stmt
    |   inquire_stmt  
	|	nullify_stmt
	|	open_stmt
	|	pointer_assignment_stmt
	|	print_stmt
	|	read_stmt
	|	return_stmt
	|	rewind_stmt
	|	stop_stmt
	|	wait_stmt
	|	where_stmt
	|	write_stmt
	|	arithmetic_if_stmt
	|	computed_goto_stmt
    |   assign_stmt 
    |   assigned_goto_stmt
    |   pause_stmt*/
  ;		

// $>

// $<section 3

// R304
name returns [string *res]
  :  T_IDENT		{ res = ACTIONS->name($T_IDENT); }
  ;
	
	
// $<3.2.2 constants

// R305
constant
returns [ExpressionBase *res]
  :  literal_constant 	{ res = $literal_constant.res; }
  |  T_IDENT 		{ res = ACTIONS->constant($T_IDENT); }
  ;
	
// R306
literal_constant
returns [StrictLiteralExpression *res]
  :  int_literal_constant 	{ res = $int_literal_constant.res; }
  |  char_literal_constant 	{ res = $char_literal_constant.res; }
  |  real_literal_constant	{ res = $real_literal_constant.res; }
  ;



// R308
int_constant
returns [ExpressionBase *res]
  :  int_literal_constant 	{ $int_literal_constant.res; }
  |  T_IDENT 			{ res = ACTIONS->int_constant($T_IDENT); }
  ;

// R309
char_constant
returns [ExpressionBase *res]
  :  char_literal_constant 	{ res = $char_literal_constant.res; }
  |  T_IDENT 			{ res = ACTIONS->char_constant($T_IDENT); }
  ;
  
// $>

// $>

// $>

// R313
label 
returns [string *res]
  :  T_DIGIT_STRING	 { res = ACTIONS->label($T_DIGIT_STRING); }
  ;

// $<section 4

// R401
type_spec
returns [TypeBase *res]
  :  intrinsic_type_spec { res = $intrinsic_type_spec.res; }
  |  derived_type_spec { res = $derived_type_spec.res; }
  ;   

// R402
type_param_value
returns [TypeParam *res]
  :  expr 	 { res = ACTIONS->type_param_value($expr.res); }
  |  T_ASTERISK  { res =ACTIONS->type_param_value(TypeParam::TV_ASSUMED); }
  |  T_COLON     { res = ACTIONS->type_param_value(TypeParam::TV_DEFERRED); }
  ;


// $<4.4 intrinsic types

// R403
intrinsic_type_spec
returns [TypeBase *res]
  :  ^(T_INTRINSIC_TYPE_SPEC T_INTEGER kind_selector?)
     	{ res = ACTIONS->intrinsic_type_spec(Grammar::IntrinsicTypes::F_INTEGER, $kind_selector.res);}
 
  |  ^(T_INTRINSIC_TYPE_SPEC T_REAL kind_selector?)
	{ res = ACTIONS->intrinsic_type_spec(Grammar::IntrinsicTypes::F_REAL, $kind_selector.res);}
 
  |  ^(T_INTRINSIC_TYPE_SPEC T_DOUBLEPRECISION)
  	{ res = ACTIONS->intrinsic_type_spec(Grammar::IntrinsicTypes::F_DOUBLE_PRECISION);}
 
  |  ^(T_INTRINSIC_TYPE_SPEC T_COMPLEX kind_selector?)
	{ res = ACTIONS->intrinsic_type_spec(Grammar::IntrinsicTypes::F_COMPLEX, $kind_selector.res);}
 
  |  ^(T_INTRINSIC_TYPE_SPEC T_DOUBLECOMPLEX)
	{ res = ACTIONS->intrinsic_type_spec(Grammar::IntrinsicTypes::F_DOUBLE_COMPLEX);}	
 
  |  ^(T_INTRINSIC_TYPE_SPEC T_CHARACTER (char_selector {res = ACTIONS->intrinsic_type_spec($char_selector.len, $char_selector.kind);} )? )

  |  ^(T_INTRINSIC_TYPE_SPEC T_LOGICAL kind_selector?)
	{ res = ACTIONS->intrinsic_type_spec(Grammar::IntrinsicTypes::F_LOGICAL, $kind_selector.res); }
  ;

// R404
kind_selector
returns [int *res]
  :  ^(T_KIND_SELECTOR (expr   {  res = ACTIONS->kind_selector($expr.res); }) ) 
  ;

// $<4.4.1 Integer type


// R406
int_literal_constant
returns [StrictLiteralExpression* res]
  : ^(T_DIGIT_STRING kind_param?)
       {  res = ACTIONS->int_literal_constant($T_DIGIT_STRING, $kind_param.res);  }
  ;

// R407
kind_param
returns [TypeParam *res]
  : expr { res = ACTIONS->kind_param($expr.res); }
  ;	


// $>


// $<4.4.2 Real Type

// R417 
real_literal_constant
returns [StrictLiteralExpression* res]
  :  ^(T_PERIOD_EXPONENT kind_param?)
       { res = ACTIONS->real_literal_constant($T_PERIOD_EXPONENT, $kind_param.res); }       
  ;

// $>

// $<4.4.4 Character Type

// R424
char_selector
returns [TypeParam *len, TypeParam *kind]
  :  ^(T_CHAR_SELECTOR 
        ^((p1=T_LEN | p2=T_KIND) t=type_param_value  
                             {                                                                                                                           
                                if ($p1) $len = $t.res;
                                else $kind = $t.res;  
                             }
         )
       (^((p3=T_LEN | p4=T_KIND) t=type_param_value
                             {
                                // TODO need to make checking
                                // PPS : fucking ANTLR, it doesn't support russian encoding (;,;)
                                if ($p3) $len = $t.res;
                                else $kind = $t.res;  
                             }       
       ))? )
  ;

// R425
length_selector
returns [TypeParam *res]
  :  type_param_value
  ;

// R426
char_length
returns [TypeParam *res]
  :  type_param_value
       { 
	  res = $type_param_value.res; 
	  res->setKind(TypeParam::TP_LEN);
       } 
  ;

scalar_int_literal_constant
returns [StrictLiteralExpression* res]
  :  int_literal_constant
       { res = $int_literal_constant.res; }
  ;

//R427	
char_literal_constant
returns [StrictLiteralExpression *res]
  :  ^(T_CHAR_CONSTANT (p=T_DIGIT_STRING)?)  
       { 
         if (p) res = ACTIONS->char_literal_constant(true, $T_CHAR_CONSTANT, $T_DIGIT_STRING);
         else res = ACTIONS->char_literal_constant($T_CHAR_CONSTANT);
       }
  |  ^(T_CHAR_CONSTANT T_IDENT) 
       { res = ACTIONS->char_literal_constant(false, $T_CHAR_CONSTANT, $T_IDENT); }		
  ;

// $>

// $>

// $<4.5 Derived types

// $<4.5.1 Derived-type definition

// R429
derived_type_def
returns [Declarations *res]
  :  ^(T_DERIVED_TYPE_DEF typeName=derived_type_stmt private_or_sequence* 
  	(members=component_part)? end_type_stmt[$typeName.res])
      {
         res = ACTIONS->derived_type_def($typeName.res, $members.res);
      }
  ;

// R430
derived_type_stmt
returns [string *res]
  :  ^(T_DERIVED_TYPE_STMT  T_IDENT type_attr_spec_list?)
     { res = new string(getTokenText($T_IDENT)); }
  ;
  
type_attr_spec_list
  : type_attr_spec+
  ;  
  

// R431
type_attr_spec
  :  access_spec
  |  ^(T_EXTENDS T_IDENT)
  |  T_ABSTRACT
  |  ^(T_BIND T_IDENT)
  ;

// R432
private_or_sequence
  :  T_PRIVATE
  |  T_SEQUENCE
  ;

// R433
end_type_stmt[string *name]
  :  ^(T_ENDTYPE T_IDENT?)
  ;

// $>

// $<4.5.3 Components

// R438
component_part
returns [list<StructMemberDescriptor*> *res]
@init 
{
  res = new list<StructMemberDescriptor*>;
}
  :  (comp=data_component_def_stmt 
     { 
       typedef list<StructMemberDescriptor *>::iterator It;
       for (It it = $comp.res->begin(); it != $comp.res->end(); ++it)   
          res->push_back(*it); 
       delete $comp.res;   
     } )+
  ;
  
// R440
data_component_def_stmt
returns [list<StructMemberDescriptor *> *res]
@init 
{ 
  list<Attribute*> *attributes = new list<Attribute*>; 
  res = new list<StructMemberDescriptor *>; 
}
@after 
{ 
  delete type; 
  
  typedef list<Attribute*>::iterator It;	
  for (It i = attributes->begin(); i != attributes->end(); ++i) delete *i;
  delete attributes;  
}
  :  ^(T_DATA_COMP_DEF_STMT type=declaration_type_spec 
	  			{ if (type == 0) return res; }  			     		
		  	   (next=component_decl[type, attributes] 
	     			{ if ($next.res) res->push_back($next.res); } )+      		
  			   )    		
  ;
   
// R442
component_decl[TypeBase *newEntityType, list<Attribute*> *attributes]
returns [StructMemberDescriptor *res]
  :  ^(T_COMP_DECL T_IDENT arraySpec=array_spec? char_length? initialization?)
  		{ res = ACTIONS->component_decl(newEntityType->clone(), 
					$attributes,   
					$T_IDENT, 
					$char_length.res, 
					$initialization.res,
					$array_spec.res); }
  ;

// $<4.5.8 Derived-type specifier

// R455
derived_type_spec
returns [DeclaredType *res]
  :  T_IDENT 
     { res = ACTIONS->derived_type_spec($T_IDENT); }
  ;
  
type_param_spec_list
  :  type_param_spec+
  ;

// R456
type_param_spec
  :  ^(T_TYPE_PARAM_SPEC name? type_param_value)
  ;


// $>


// $<4.5.9 Construction of derived-type values

structure_constructor
  :  ^(T_STRUCTURE_CTOR T_IDENT type_param_spec_list?)
  ;

// $>


// $>

// $<4.7 Construction of array values

// R465
array_constructor
returns [ArrayConstructorExpression *res]
  :  ^(T_ARRAY_CONSTRUCTOR ac_spec)
       { res = $ac_spec.res; }
  ;

// R466
ac_spec 
returns [ArrayConstructorExpression *res]
  :  ^(T_AC_SPEC type_spec (ac_value_list)?) 
       { }
  
  |  ^(T_AC_SPEC ac_value_list) 
       { }
  ;		
    
// R469
ac_value
  :  expr
  |  ac_implied_do
  ;

ac_value_list
  :  ^(T_AC_VALUE_LIST ac_value+)
 ;

// R470
ac_implied_do
  : ^(T_AC_INPLIED_DO ac_value_list ac_implied_do_control)
  ;    

// R471
ac_implied_do_control
  :  ^(T_AC_IMPLIED_DO_CONTROL expr expr expr?)
  ;

// $>

// $>

// $>


// $<Section 5

// $<5.1 Type declaration statements

// R501
type_declaration_stmt
returns[Declarations *res]
@init { 
  list<Attribute*> *attributes = new list<Attribute*>; 
  res = new Declarations;
}
@after 
{ 
  delete type; 
  
  typedef list<Attribute*>::iterator It;	
  for (It i = attributes->begin(); i != attributes->end(); ++i) delete *i;
  delete attributes;
  
}
  :  ^(T_TYPE_DECLARATION_STMT 
  		type=declaration_type_spec 
  			{ if (type == 0) return res; }
     		(attr=attr_spec 
     			{if (attr) attributes->push_back(attr); } )* 
     		(next=entity_decl[type, attributes] 
     			{ ACTIONS->type_declaration_stmt(res, next); } )+)     		    
  ;


// R502
declaration_type_spec
returns [TypeBase *res]
  :  intrinsic_type_spec 	{ res = $intrinsic_type_spec.res;  }			
  | ^(T_TYPE derived_type_spec) { res = $derived_type_spec.res; }
/*	|	T_CLASS	T_LPAREN derived_type_spec T_RPAREN
			{ action.declaration_type_spec($T_CLASS, 
                IActionEnums.DeclarationTypeSpec_CLASS); }
	|	T_CLASS T_LPAREN T_ASTERISK T_RPAREN
			{ action.declaration_type_spec($T_CLASS,
                IActionEnums.DeclarationTypeSpec_unlimited); }*/
  ;
	
// R503
attr_spec
returns [Attribute *res]

	:	access_spec { res = $access_spec.res; }
	|	T_ALLOCATABLE		{ res = new Attribute(Grammar::ATTR_ALLOCATABLE);  }
	|	T_ASYNCHRONOUS		{ res = new Attribute(Grammar::ATTR_ASYNCHRONOUS); }
	|	T_DIMENSION T_LPAREN array_spec T_RPAREN
					{ 
					  res = new Attribute(Grammar::ATTR_DIMENSION);
					  res->arraySpec = $array_spec.res;					
					}
	|	T_EXTERNAL		{ res = new Attribute(Grammar::ATTR_EXTERNAL); }
	/*|	T_INTENT T_LPAREN intent_spec T_RPAREN
						{ action.attr_spec($T_INTENT, 
                IActionEnums.AttrSpec_INTENT); }*/
	|	T_INTRINSIC		{ res = new Attribute(Grammar::ATTR_INTRINSIC); }
	/*|	language_binding_spec		
						{ action.attr_spec(null, 
                IActionEnums.AttrSpec_language_binding); }*/
	|	T_OPTIONAL		{ res = new Attribute(Grammar::ATTR_OPTIONAL); }
	|	T_PARAMETER		{ res = new Attribute(Grammar::ATTR_PARAMETER); }
	|	T_POINTER		{ res = new Attribute(Grammar::ATTR_POINTER); }
	|	T_PROTECTED		{ res = new Attribute(Grammar::ATTR_PROTECTED); }
	|	T_SAVE			{ res = new Attribute(Grammar::ATTR_SAVE); }
	|	T_TARGET		{ res = new Attribute(Grammar::ATTR_TARGET); }
	|	T_VALUE			{ res = new Attribute(Grammar::ATTR_VALUE); }
	|	T_VOLATILE		{ res = new Attribute(Grammar::ATTR_VOLATILE); }
	;
	

// R504
entity_decl[TypeBase *newEntityType, list<Attribute*> *attributes]
returns [DeclarationBase *res]
  :  ^(T_ENTITY_DECL T_IDENT arraySpec=array_spec? char_length? initialization?)
  		{ res = ACTIONS->entity_decl(newEntityType->clone(), 
					$attributes,   
					$T_IDENT, 
					$char_length.res, 
					$initialization.res,
					$array_spec.res); }
  ;


// R506
initialization
returns [ExpressionBase *res]
  :  ^(T_EQUALS expr) { res = $expr.res; }
  //|	T_EQ_GT null_init	{ action.initialization(false, true); }
  ;


// R508
access_spec 
returns [Attribute *res]
  :  T_PUBLIC 	{ res = new Attribute(Grammar::ATTR_PUBLIC);  }
  |  T_PRIVATE 	{ res = new Attribute(Grammar::ATTR_PRIVATE); }
  ;

// R510
array_spec
returns [list<ArrayDimensionExpression*> *res]
@init { res = new list<ArrayDimensionExpression*>; }
@after { ACTIONS->array_spec(res);} 
  : ^(T_ARRAY_SPEC (array_spec_element { res->push_back($array_spec_element.res); })+)
  ;

array_spec_element
returns [ArrayDimensionExpression *res]
@init { res = new ArrayDimensionExpression; }
  :
     ^(T_ARRAY_SPEC_ELEMENT e1=expr {if (e1) res->setLowerBound(e1); } (e2=expr { if (e2) res->setUpperBound(e2); })?) 
  |  ^(T_ARRAY_SPEC_ELEMENT T_ASTERISK) { res->setUpperBound(new DefferedShapeExpression); }
  |  ^(T_ARRAY_SPEC_ELEMENT T_COLON) 
  ;


// $>

// $>

// $<Section 6

variable
returns [ExpressionBase *res] 
  :  designator { res = $designator.res; }
  ;


// R603
designator	
returns[ExpressionBase *res]
  :  ^(T_DEGIGNATOR data_ref { res = $data_ref.res; } substring_range?)
  
  |  ^(T_DEGIGNATOR char_literal_constant substring_range)
  ;
	
// R611
substring_range
  :  expr? T_COLON expr?
  ;
	
// R612
data_ref
returns[ExpressionBase *res]
  :  ^(T_DATA_REF (part=part_ref[res] { res = part; } )+ )
  ;
		
part_ref[ExpressionBase *structPointer]
returns[ExpressionBase *res]
  :  ^(T_PART_REF T_IDENT ll=section_subscript_list?)
       { res = ACTIONS->part_ref(structPointer, $T_IDENT); }
  ;

section_subscript_list
returns []
  :  section_subscript (T_COMMA section_subscript)*  
  ;

//R619 
section_subscript 
  :  e1=expr (T_COLON e2=expr? (T_COLON e3=expr)?)?
  |  T_COLON e2=expr? (T_COLON e3=expr)?
  ;

// $>


// $<Section 7

// $<7.1 Expressions

/*
 * Section 7:
 */

// R722
expr
returns [ExpressionBase *res]
  : ^(T_DEFINED_BINARY_OP e1=expr e2=expr)	{ res = ACTIONS->expr($T_DEFINED_BINARY_OP, e1, e2); }
  | ^(T_EQV e1=expr e2=expr) 			{ res = ACTIONS->expr($T_EQV, e1, e2); }
  | ^(T_NEQV e1=expr e2=expr) 			{ res = ACTIONS->expr($T_NEQV, e1, e2); }
  | ^(T_AND e1=expr e2=expr) 			{ res = ACTIONS->expr($T_AND, e1, e2); }
  | ^(T_NOT e1=expr) 				{ res = ACTIONS->expr($T_NOT, e1); }
  | ^(T_OR e1=expr e2=expr)			{ res = ACTIONS->expr($T_OR, e1, e2); }
  
  // relative op:
  | ^(T_EQ e1=expr e2=expr) 			{ res = ACTIONS->expr($T_EQ, e1, e2); }		 	
  | ^(T_NE e1=expr e2=expr) 			{ res = ACTIONS->expr($T_NE, e1, e2); }		 	
  | ^(T_LT e1=expr e2=expr)			{ res = ACTIONS->expr($T_LT, e1, e2); }		 	
  | ^(T_LE e1=expr e2=expr)			{ res = ACTIONS->expr($T_LE, e1, e2); }		 	
  | ^(T_GT e1=expr e2=expr) 			{ res = ACTIONS->expr($T_GT, e1, e2); }		 	
  | ^(T_GE e1=expr e2=expr)			{ res = ACTIONS->expr($T_GE, e1, e2); }		 	
  | ^(T_EQ_EQ e1=expr e2=expr) 			{ res = ACTIONS->expr($T_EQ_EQ, e1, e2); }	 	
  | ^(T_SLASH_EQ e1=expr e2=expr) 		{ res = ACTIONS->expr($T_SLASH_EQ, e1, e2); }	 	
  | ^(T_LESSTHAN e1=expr e2=expr) 		{ res = ACTIONS->expr($T_LESSTHAN, e1, e2); }		
  | ^(T_LESSTHAN_EQ e1=expr e2=expr) 		{ res = ACTIONS->expr($T_LESSTHAN_EQ, e1, e2); }	
  | ^(T_GREATERTHAN e1=expr e2=expr) 		{ res = ACTIONS->expr($T_GREATERTHAN, e1, e2); }
  | ^(T_GREATERTHAN_EQ e1=expr e2=expr) 	{ res = ACTIONS->expr($T_GREATERTHAN_EQ, e1, e2); }
 
  | ^(T_SLASH_SLASH e1=expr e2=expr)		{ res = ACTIONS->expr($T_SLASH_SLASH, e1, e2); }
  | ^(T_PLUS e1=expr e2=expr)			{ res = ACTIONS->expr($T_PLUS, e1, e2); }  
  | ^(T_MINUS e1=expr e2=expr?)			{ res = ACTIONS->expr($T_MINUS, e1, e2); }
  | ^(T_ASTERISK e1=expr e2=expr)		{ res = ACTIONS->expr($T_ASTERISK, e1, e2); }
  | ^(T_SLASH e1=expr e2=expr)			{ res = ACTIONS->expr($T_SLASH, e1, e2); }

  | ^(T_DEFINED_OP e1=expr)			{ res = ACTIONS->expr($T_DEFINED_OP, e1); }
 
  |  literal_constant 				{ res = $literal_constant.res;  }
  |  designator 				{ res = $designator.res; 	}
  |  array_constructor 				{ res = $array_constructor.res; }
  ;
    
    
// R734
assignment_stmt
returns [ExpressionStatement *res]
  :  ^(T_ASSIGNMENT_STMT variable expr)
       { res = ACTIONS->assignment_stmt($variable.res, $expr.res); }
  ;         
    
// $>

// $>

// $<Section 8

// R801
block
returns[BlockStatement *res]
@init 
{
  res = new BlockStatement;
}
  :  (execPart=execution_part_construct { res->addLast($execPart.res); } )*
  ;

// $<8.1.2 IF construct

// R802
if_construct
returns [IfStatement *res]
@init 
{ 
  res = new IfStatement; 
  IfStatement *buf, *curIf;
  
  buf = res;
  
  string *firstifname, *endifname;
  list<string *> middleifnames;  
}
@after
{
  delete firstifname;
  delete endifname;
  typedef list<string *>::iterator It;
  for (It it = middleifnames.begin(); it != middleifnames.end(); ++it)
    delete *it; 
}
  :  (
  		if_then_stmt 
  		{ 
  		  firstifname = $if_then_stmt.ifname;
  		  res->setCondition($if_then_stmt.res); 
  		}
  		b1=block { res->setThenBody($b1.res); }
  		(else_if_stmt b2=block 
  		  { 
  		     middleifnames.push_back($else_if_stmt.ifname);
  		       		       		     
  		     curIf = new IfStatement;
  		     curIf->setCondition($else_if_stmt.res);
  		     curIf->setThenBody($b2.res);
  		     
  		     BlockStatement *ifBlock = new BlockStatement;
  		     ifBlock->addLast(curIf);
  		     res->setElseBody(ifBlock); 
  		     res = curIf;
  		  } )*
  		(else_stmt { endifname = $else_stmt.res; }
  		 b3=block { res->setElseBody($b3.res); } )? 
  		end_if_stmt)
     {
       res = buf;
       // need to validate!!!!
       
     }
  ;

// R803
if_then_stmt
returns [ExpressionBase *res, string *ifname]
@init { retval.res = 0; retval.ifname = 0; }
  :  ^(T_IF_THEN expr { retval.res=$expr.res; } (T_IDENT { retval.ifname = new string(getTokenText($T_IDENT)); })?)  
  ;

// R804
else_if_stmt
returns [ExpressionBase *res, string *ifname]
@init { retval.res = 0; retval.ifname = 0; }
  :  ^(T_ELSEIF expr { retval.res=$expr.res; } (T_IDENT { retval.ifname = new string(getTokenText($T_IDENT)); })?)  
  ;

// R805
else_stmt
returns [string *res]
  :  ^(T_ELSE (T_IDENT { res = new string(getTokenText($T_IDENT)); })?)
  ;

// R806
end_if_stmt
returns [string *res]
  :  ^(T_ENDIF (T_IDENT { res = new string(getTokenText($T_IDENT)); })?)  
  ;

// $>

// $<8.1.6 DO construct

// R825
do_construct
returns [StatementBase *res]
  :  ^(T_DO_CONSTRUCT do_stmt block end_do)
     {        
	ACTIONS->do_construct($do_stmt.res, $do_stmt.doName, $do_stmt.doLabel,
	      $block.res,
	      $end_do.endDoStmt, $end_do.endDoName, $end_do.endDoLabel);
	res = $do_stmt.res;
     }
  ;

// R827
do_stmt
returns [string *doName, string *doLabel, StatementBase *res]
@init { retval.doName = 0; retval.doLabel=0; retval.res = 0; }
  :  ^(T_DO (T_IDENT)? label? (loop_control)?)     
      {
         if ($T_IDENT) $doName  = new string(getTokenText($T_IDENT));
         $doLabel = $label.res;
         $res     = $loop_control.res;
      }
  ;

// R829 inlined in R827
loop_control
returns [StatementBase *res]
  :  ^(T_WHILE expr)
      { res = new WhileStatement(true, $expr.res); }     
  |  ^(T_FOR_LOOP_CONTROL do_variable e1=expr e2=expr (e3=expr)?)
      { 
         BasicCallExpression *initExpr = new BasicCallExpression(BasicCallExpression::BCK_ASSIGN);    
         
         if ($do_variable.res)  
           initExpr->addArgument($do_variable.res);
	 else  
	   initExpr->addArgument(new EmptyExpression);
         initExpr->addArgument($e2.res);
         res = new ForStatement(initExpr, $e2.res, $e3.res);
      }
  ;

// R831
do_variable
returns [ExpressionBase *res]
  :  variable { res = $variable.res; } 
  ;

// R833
end_do
returns [string *endDoName, string *endDoLabel, StatementBase *endDoStmt]
@init { retval.endDoName=0; retval.endDoLabel=0; retval.endDoStmt=0; }
  :  ^(T_DO_TERM_ACTION_STMT action_stmt label?)
     {
        $endDoStmt=$action_stmt.res;
        $endDoLabel=$label.res;
     }
  |  ^(T_ENDDO (T_IDENT)? label?)
     {
        if ($T_IDENT) $endDoName= new string(getTokenText($T_IDENT));
        $endDoLabel=$label.res;       
     }
  ;

// R843
cycle_stmt
returns [ContinueStatement *res]
  :  ^(T_CYCLE T_IDENT?)
      {
        res = new ContinueStatement;
      }
  ;

// R844
exit_stmt
returns [BreakStatement *res]
  :  ^(T_EXIT T_IDENT?)
      {
        res = new BreakStatement;
      }  
  ;

// $>


// $>


// $<11.1 Main program

// R1101
main_program
returns [SubroutineDeclaration *res]
@init  
{ 
  NEAREST_DECLARATIONS = new Declarations();  
}
@after 
{ 
  NEAREST_DECLARATIONS = 0;  
  TREE_PARSER_BASE->clearLabelsTable();
}
  :  ^(T_MAIN_PROGRAM progName=program_stmt specification_part execution_part end_program_stmt[progName]) 
	{
  	  TREE_PARSER_BASE->resolveBranchingOperators();
	  res = ACTIONS->main_program(progName, $specification_part.res, $execution_part.res); 
	}
  ;


// R1102
program_stmt
returns [string *res]
  :  ^(T_PROGRAM T_IDENT)
	{ res = ACTIONS->program_stmt($T_IDENT); }		
  ;

// R1103
end_program_stmt[string *program_name]
  :  ^(T_ENDPROGRAM (endName=T_IDENT { ACTIONS->end_program_stmt(program_name, new string(getTokenText(endName))); })? )
  ;		
// $>
